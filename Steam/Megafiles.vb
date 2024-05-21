'Written for games that use Megafiles.
'Command & Conquer™ Remastered Collection https://store.steampowered.com/app/1213210
'8-Bit Hordes https://store.steampowered.com/app/497850
'9-Bit Armies: A Bit Too Far https://store.steampowered.com/app/1439750
'STAR WARS™ Empire at War - Gold Pack https://store.steampowered.com/app/32470
'Battle for Graxia https://steamcommunity.com/app/90530
'Victory Command https://steamcommunity.com/app/360480
'Universe at War: Earth Assault https://steamcommunity.com/app/10430
Imports System.Collections.Generic
Imports System.IO
Imports System.IO.Compression

    Class Program
        Private Shared br As BinaryReader

        Shared Sub Main(args As String())
            br = New BinaryReader(File.OpenRead(args(0)))

            If New String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetChars(br.ReadBytes(8))) <> "ÿÿÿÿ¤p}?" Then
                Throw New System.Exception("This is not a Megafile.")
            End If

            Dim fileDataTableStart As Integer = br.ReadInt32()
            Dim fileCount As Integer = br.ReadInt32()
            Dim namesCount As Integer = br.ReadInt32()
            Dim fileTableStart As Integer = br.ReadInt32()

            Dim names As New List(Of String)()
            For i As Integer = 0 To namesCount - 1
                names.Add(New String(br.ReadChars(br.ReadInt16())))
            Next

            br.ReadInt16()

            Dim data As New List(Of FileData)()
            For i As Integer = 0 To fileCount - 1
                data.Add(New FileData())
            Next

            Dim path As String = Path.GetDirectoryName(args(0))
            Dim n As Integer = 0
            For Each file As FileData In data
                br.BaseStream.Position = file.start
                Dim bw As BinaryWriter
                If Path.GetDirectoryName(names(n)) <> "" Then
                    Directory.CreateDirectory(path & "//" & Path.GetDirectoryName(names(n)))
                    bw = New BinaryWriter(File.Create(path & "//" & names(n)))
                Else
                    Directory.CreateDirectory(path & "//Data//" & Path.GetFileNameWithoutExtension(args(0)))
                    bw = New BinaryWriter(File.Create(path & "//Data//" & Path.GetFileNameWithoutExtension(args(0)) & "//" & names(n)))
                End If
                bw.Write(br.ReadBytes(file.size))
                bw.Close()

                Select Case Path.GetExtension(names(n))
                    Case ".APF", ".CPD", ".GPD", ".SOB", ".TED", ".TER", ".ALO", ".ALA"
                        Decompress(path & "//" & names(n))
                End Select
                n += 1
            Next
        End Sub

        Private Class FileData
            Private unknown As Single = br.ReadSingle()
            Private number As Integer = br.ReadInt32()
            Public size As Integer = br.ReadInt32()
            Public start As Integer = br.ReadInt32()
            Private unknown2 As Integer = br.ReadInt32()
        End Class

        Public Shared Sub Decompress(file As String)
            Dim decmps As New BinaryReader(File.OpenRead(file))
            decmps.BaseStream.Position = 16
            Dim size As Integer = decmps.ReadInt32()
            decmps.BaseStream.Position += 18
            Dim path As String = Path.GetDirectoryName(file) & "\extracted\"
            Directory.CreateDirectory(path)
            Dim fs As FileStream = File.Create(path & Path.GetFileName(file))
            Using ds As New DeflateStream(New MemoryStream(decmps.ReadBytes((size - 2))), CompressionMode.Decompress)
                ds.CopyTo(fs)
            End Using
            fs.Close()
        End Sub
    End Class


