'Written for Florensia. https://store.steampowered.com/app/384030
Imports System.IO

    Module Program
        Private br As BinaryReader
        Private path As String

        Sub Main(args As String())
            br = New BinaryReader(File.OpenRead(args(0)))
            Dim count As Integer = br.ReadInt32()

            Dim subfiles As New List(Of Subfile)()
            For i As Integer = 0 To count - 1
                subfiles.Add(New Subfile())
                br.BaseStream.Position += 28 'unknown
            Next

            Dim path As String = Path.GetDirectoryName(args(0)) & "//" & Path.GetFileNameWithoutExtension(args(0))
            Directory.CreateDirectory(path)

            For Each file As Subfile In subfiles
                br.BaseStream.Position = file.start
                Dim bw As New BinaryWriter(File.Create(path & "//" & file.name))
                bw.Write(br.ReadBytes(file.size))
                bw.Close()
            Next
        End Sub

        Class Subfile
            Public name As String
            Public start As Integer
            Public size As Integer

            Public Sub New()
                name = New String(br.ReadChars(260)).TrimEnd(ChrW(0))
                start = br.ReadInt32()
                size = br.ReadInt32()
            End Sub
        End Class
    End Module



