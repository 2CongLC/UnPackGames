Imports System
Imports System.IO

    Class Program
        Shared Sub Main(ByVal args As String())
            Dim br As New BinaryReader(File.OpenRead(args(0)))

            If New String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetChars(br.ReadBytes(&H100))).TrimEnd(CChar(0), "ý"c) <> "EyedentityGames Packing File 0.1" Then
                Throw New Exception("This is not an EyedentityGames Packing File.")
            End If

            br.ReadInt32() 'Unknown
            Dim fileCount As Integer = br.ReadInt32()
            Dim path As String = Path.GetDirectoryName(args(0))
            br.BaseStream.Position = br.ReadInt32()
            Dim subfiles As New System.Collections.Generic.List(Of Subfile)()

            For i As Integer = 0 To fileCount - 1
                subfiles.Add(New Subfile() With {
                    .name = New String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetChars(br.ReadBytes(&H100))).TrimEnd(CChar(0), "ý"c),
                    .size1 = br.ReadInt32(),
                    .unknown = br.ReadInt32(),
                    .size2 = br.ReadInt32(), 'I don't know why it's here twice.
                    .start = br.ReadInt32()
                })
                If subfiles(subfiles.Count - 1).size1 <> subfiles(subfiles.Count - 1).size2 Then
                    Throw New Exception("Fuck!")
                End If
                br.BaseStream.Position += 44
            Next

            For Each file As Subfile In subfiles
                br.BaseStream.Position = file.start
                Directory.CreateDirectory(path & "//" & Path.GetDirectoryName(file.name))
                Using bw As New BinaryWriter(File.Create(path & "//" & file.name))
                    bw.Write(br.ReadBytes(file.size1))
                End Using
            Next
        End Sub

        Class Subfile
            Public name As String
            Public size1 As Integer
            Public unknown As Integer
            Public size2 As Integer 'I don't know why it's here twice.
            Public start As Integer
        End Class
    End Class


