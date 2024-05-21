Imports System.IO

    Class Program
        Public Shared br As BinaryReader

        Shared Sub Main(ByVal args() As String)
            br = New BinaryReader(File.OpenRead(args(0)))

            If New String(br.ReadChars(4)) <> "MDPK" Then
                Throw New System.Exception("This is not Warspear Online's pak file.")
            End If

            br.ReadInt16() 'Version?
            Dim count As Integer = br.ReadInt32()
            Dim date As String = New String(br.ReadChars(10))
            br.BaseStream.Position += 6

            Dim subfiles As New List(Of Subfile)()
            For i As Integer = 0 To count - 1
                subfiles.Add(New Subfile())
            Next

            Dim path As String = Path.GetDirectoryName(args(0)) & "//" & Path.GetFileNameWithoutExtension(args(0)) & "//"
            For Each file As Subfile In subfiles
                Directory.CreateDirectory(path & Path.GetDirectoryName(file.name))
                br.BaseStream.Position = file.start
                Dim bw As New BinaryWriter(File.Create(path & file.name))
                bw.Write(br.ReadBytes(file.size))
                bw.Close()
            Next
        End Sub

        Class Subfile
            Public start As Integer = br.ReadInt32()
            Public size As Integer = br.ReadInt32()
            Dim unknown As Integer = br.ReadInt32()
            Dim unknown2 As Byte = br.ReadByte()
            Public name As String = New String(br.ReadChars(55)).TrimEnd(ControlChars.NullChar)
        End Class
    End Class


