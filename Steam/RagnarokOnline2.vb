Imports System.IO
Imports System.IO.Compression
Imports System.Collections.Generic

    Module Program
        Private br As BinaryReader
        Private path As String

        Sub Main(args As String())
            br = New BinaryReader(File.OpenRead(args(0)))
            If New String(br.ReadChars(8)) <> "VDISK1.1" Then
                Throw New Exception("Not a Ragnarok Online VDK file.")
            End If

            path = Path.GetDirectoryName(args(0)) & "\"
            Dim unknown1 As Integer = br.ReadInt32()
            Dim fileCount As Integer = br.ReadInt32()
            Dim folderCount As Integer = br.ReadInt32()
            Dim tableStart As Integer = br.ReadInt32() + 145
            Dim unknown2 As Integer = br.ReadInt32()

            br.BaseStream.Position = tableStart
            Dim table As List(Of TableEntry) = Table()

            For Each file As TableEntry In table
                br.BaseStream.Position = file.start
                Subfile(file.name)
            Next
        End Sub

        Function Table() As List(Of TableEntry)
            Dim count As Integer = br.ReadInt32()
            Dim table As New List(Of TableEntry)()
            For i As Integer = 0 To count - 1
                table.Add(New TableEntry())
            Next
            Return table
        End Function

        Public Class TableEntry
            Public name As String = New String(br.ReadChars(260)).TrimEnd(ChrW(0))
            Public start As Integer = br.ReadInt32()
        End Class

        Sub Subfile(fullName As String)
            Dim isFolder As Byte = br.ReadByte()
            Dim name As String = New String(br.ReadChars(128)).TrimEnd(ChrW(0))
            Dim sizeUncompressed As Integer = br.ReadInt32()
            Dim sizeCompressed As Integer = br.ReadInt32()
            Dim unknown1 As Integer = br.ReadInt32()
            Dim endPos As Integer = br.ReadInt32()

            Directory.CreateDirectory(path & Path.GetDirectoryName(fullName))

            If isFolder = 1 Then
                Throw New Exception("Fuck!")
            End If

            Using fs As FileStream = File.Create(path & fullName)
                If sizeUncompressed = sizeCompressed Then
                    Using bw As New BinaryWriter(fs)
                        bw.Write(br.ReadBytes(sizeUncompressed))
                    End Using
                    Return
                End If
                br.ReadInt16()
                Using ds As New DeflateStream(New MemoryStream(br.ReadBytes(sizeCompressed)), CompressionMode.Decompress)
                    ds.CopyTo(fs)
                End Using
            End Using
        End Sub
    End Module


