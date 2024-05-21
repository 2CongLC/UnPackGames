'Written for Captain Lycop : Invasion of the Heters. https://store.steampowered.com/app/525070
Imports System
Imports System.IO
Imports System.IO.Compression

    Module Program
        Dim br As BinaryReader
        Dim size, n As Integer
        Dim path, type As String

        Sub Main(args As String())
            br = New BinaryReader(File.OpenRead(args(0)))
            br.BaseStream.Position = 20

            n = 0
            path = Path.GetDirectoryName(args(0)) & "\" & Path.GetFileNameWithoutExtension(args(0))
            Directory.CreateDirectory(path)
            While br.BaseStream.Position < br.BaseStream.Length
                Dim variable = br.ReadBytes(4)
                Array.Reverse(variable)
                size = BitConverter.ToInt32(variable, 0)

                type = GetType()
                If type = ".zlib" Then
                    variable = br.ReadBytes(4)
                    Array.Reverse(variable)
                    Dim sizeUncompressed = BitConverter.ToInt32(variable, 0)

                    br.ReadInt16()
                    Dim ms As New MemoryStream()
                    Using ds = New DeflateStream(New MemoryStream(br.ReadBytes(size - 6)), CompressionMode.Decompress)
                        ds.CopyTo(ms)
                    End Using

                    Dim position = br.BaseStream.Position
                    size = sizeUncompressed

                    br = New BinaryReader(ms)
                    br.BaseStream.Position = 0
                    type = GetType()
                    WriteFile()
                    br = New BinaryReader(File.OpenRead(args(0)))
                    br.BaseStream.Position = position
                Else
                    WriteFile()
                End If
                n += 1
            End While
        End Sub

        Sub WriteFile()
            Dim bw As New BinaryWriter(File.Create(path & "\" & n & type))
            bw.Write(br.ReadBytes(size))
            bw.Close()
        End Sub

        Function GetType() As String
            Dim magicBytes = br.ReadBytes(4)
            br.BaseStream.Position -= 4
            Dim magic = System.Text.Encoding.UTF7.GetString(magicBytes)
            Select Case magic
                Case "OggS"
                    Return ".ogg"
                Case ChrW(&H89) & "PNG"
                    Return ".png"
                Case "RIFF"
                    Return ".wav"
                Case Else
                    br.BaseStream.Position += 4
                    magicBytes = br.ReadBytes(2)
                    If magicBytes(0) <> &H78 OrElse magicBytes(1) <> &H9C Then
                        Throw New Exception("Unrecognized subfile type.")
                    End If

                    br.BaseStream.Position -= 6
                    Return ".zlib"
            End Select
        End Function
    End Module



