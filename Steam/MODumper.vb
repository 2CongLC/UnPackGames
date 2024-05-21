Imports System
Imports System.IO

    Module Program
        Dim br As BinaryReader

        Sub Main(args As String())
            br = New BinaryReader(File.OpenRead(args(0)))
            br.ReadBytes(8)
            Dim count As Integer = br.ReadInt32() * 16 \ 8
            Dim tableStart As Integer = br.ReadInt32()
            Dim unknown As Integer = br.ReadInt32()

            br.BaseStream.Position = tableStart
            Dim strings As New List(Of [String])()
            For i As Integer = 0 To count - 1
                strings.Add(New [String]())
            Next

            Dim bw As New StreamWriter(File.Create(Path.GetDirectoryName(args(0)) & "\" & Path.GetFileNameWithoutExtension(args(0)) & ".txt"))
            For Each text As [String] In strings
                br.BaseStream.Position = text.start
                bw.WriteLine(System.Text.Encoding.Default.GetString(br.ReadBytes(text.size)))
            Next
            bw.Close()
        End Sub

        Class [String]
            Public size As Integer
            Public start As Integer

            Public Sub New()
                size = br.ReadInt32()
                start = br.ReadInt32()
            End Sub
        End Class
    End Module



