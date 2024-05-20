Imports System.IO
Imports System.IO.Compression

    Module Program
        Dim br As BinaryReader

        Sub Main(args As String())
            Dim input As FileStream = File.OpenRead(args(0))
            br = New BinaryReader(input)
            br.BaseStream.Position = 52

            Dim fileCount As Integer = br.ReadInt32()
            br.BaseStream.Position = 128

            Dim subfiles As New List(Of Subfile)()
            For i As Integer = 0 To fileCount - 1
                subfiles.Add(New Subfile())
            Next

            Dim fileData As Long = br.BaseStream.Position
            Dim n As Integer = 0
            Directory.CreateDirectory(Path.GetDirectoryName(input.Name) & "//" & Path.GetFileNameWithoutExtension(input.Name))
            For Each subfile As Subfile In subfiles
                br.BaseStream.Position = subfile.start + fileData
                Dim bw As New BinaryWriter(File.Create(Path.GetDirectoryName(args(0)) & "//" & Path.GetFileNameWithoutExtension(args(0)) & "//" & n))
                If subfile.isCompressed = 1 Then
                    Dim ms As New MemoryStream()
                    br.ReadInt16()
                    Using ds As New DeflateStream(New MemoryStream(br.ReadBytes(subfile.sizeCompressed)), CompressionMode.Decompress)
                        ds.CopyTo(ms)
                    End Using
                    br = New BinaryReader(ms)
                    br.BaseStream.Position = 0
                    bw.Write(br.ReadBytes(subfile.sizeUncompressed))
                    bw.Close()
                    br = New BinaryReader(input)
                    n += 1
                    Continue For
                ElseIf subfile.isCompressed = 2 Then
                    bw.Write(br.ReadBytes(subfile.sizeCompressed))
                    bw.Close()
                    n += 1
                    Continue For
                End If
                bw.Write(br.ReadBytes(subfile.sizeUncompressed))
                bw.Close()
                n += 1
            Next
        End Sub

        Public Class Subfile
            Public start As Long
            Public sizeCompressed As Integer
            Public sizeUncompressed As Integer
            Public isCompressed As Long

            Public Sub New()
                start = br.ReadInt64()
                sizeCompressed = br.ReadInt32()
                sizeUncompressed = br.ReadInt32()
                isCompressed = br.ReadInt64()
            End Sub
        End Class
    End Module



