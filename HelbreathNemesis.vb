'Written for Helbreath Nemesis. https://steamcommunity.com/app/2857560/
Imports System.IO

    Module Program
        Private br As BinaryReader

        Sub Main(args As String())
            br = New BinaryReader(File.OpenRead(args(0)))
            If New String(br.ReadChars(17)) <> "<Pak file header>" Then
                Throw New Exception("Please input a PAK file from Helbreath Nemesis.")
            End If

            br.ReadBytes(3)

            Dim count As Integer = br.ReadInt32()
            Dim subfiles As New List(Of Subfile)()
            For i As Integer = 0 To count - 1
                subfiles.Add(New Subfile())
            Next

            Dim path As String = Path.GetDirectoryName(args(0)) & "//" & Path.GetFileNameWithoutExtension(args(0))
            Directory.CreateDirectory(path)

            Dim n As Integer = 0
            For Each file As Subfile In subfiles
                br.BaseStream.Position = file.start
                Dim bw As New BinaryWriter(File.Create(path & "//" & n & ".Sprite"))
                bw.Write(br.ReadBytes(file.size))
                n += 1
            Next
        End Sub

        Class Subfile
            Public start As Integer
            Public size As Integer

            Public Sub New()
                start = br.ReadInt32()
                size = br.ReadInt32()
            End Sub
        End Class
    End Module


