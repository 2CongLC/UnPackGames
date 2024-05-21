Imports System.IO

    Friend Class Program
        Private Shared br As BinaryReader

        Shared Sub Main(args As String())
            br = New BinaryReader(File.OpenRead(args(0)))
            If New String(br.ReadChars(4)) <> "PAK " Then
                Throw New Exception("Not a Fields of Battle pak file.")
            End If

            Dim dataStart As Integer = br.ReadInt32()
            br.BaseStream.Position = 18
            Dim fileCount As Integer = br.ReadInt32()
            br.BaseStream.Position = 22

            Dim subfiles As New List(Of Subfile)()

            For i As Integer = 0 To fileCount - 1
                subfiles.Add(ReadSub())
            Next

            Dim path As String = Path.GetDirectoryName(args(0)) & "//" & Path.GetFileNameWithoutExtension(args(0))
            Directory.CreateDirectory(path)

            For Each file As Subfile In subfiles
                br.BaseStream.Position = file.start
                Dim bw As New BinaryWriter(File.Create(path & "//" & file.name))
                bw.Write(br.ReadBytes(file.size))
            Next
        End Sub

        Structure Subfile
            Public name As String
            Public start As Integer
            Public size As Integer
            Public isCompressed As Byte
        End Structure

        Private Shared Function ReadSub() As Subfile
            br.ReadInt16() 'usually 4
            Dim name As String = New String(br.ReadChars(64)).Trim(ChrW(0))
            br.ReadInt16()
            Dim subfile As New Subfile() With {
                .name = name,
                .start = br.ReadInt32(),
                .size = br.ReadInt32()
            }
            br.ReadInt32()
            br.ReadInt32()
            br.ReadInt32()
            subfile.isCompressed = br.ReadByte()
            Return subfile
        End Function
    End Class



