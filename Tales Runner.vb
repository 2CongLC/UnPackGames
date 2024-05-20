'Written for Tales Runner. https://store.steampowered.com/app/328060
Imports System.IO
Imports System.IO.Compression


    Module Program
        Sub Main(args As String())
            Dim br As New BinaryReader(File.OpenRead(args(0)))
            br.BaseStream.Position = 32

            Dim n As Integer = 0
            Directory.CreateDirectory(Path.GetDirectoryName(args(0)) & "//" & Path.GetFileNameWithoutExtension(args(0)))
            While br.BaseStream.Position < br.BaseStream.Length
                br.ReadInt32()
                Dim sizeUncompressed As Integer = br.ReadInt32()
                Dim sizeCompressed As Integer = br.ReadInt32()
                br.ReadSingle()
                Dim isCompressed As Integer = br.ReadInt32()

                If isCompressed = 1 Then
                    Dim ms As New MemoryStream()
                    br.ReadInt16()
                    Using ds As New DeflateStream(New MemoryStream(br.ReadBytes(sizeCompressed - 2)), CompressionMode.Decompress)
                        ds.CopyTo(File.Create(Path.GetDirectoryName(args(0)) & "//" & Path.GetFileNameWithoutExtension(args(0)) & "//" & n))
                    End Using
                    n += 1
                    Continue While
                End If
                Dim bw As New BinaryWriter(File.Create(Path.GetDirectoryName(args(0)) & "//" & Path.GetFileNameWithoutExtension(args(0)) & "//" & n))
                bw.Write(br.ReadBytes(sizeUncompressed))
                bw.Close()
                n += 1
            End While
        End Sub
    End Module



