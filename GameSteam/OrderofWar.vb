'Written for Order of War. https://store.steampowered.com/app/34600
Imports System.IO
Imports System.Text

    Module Program
        Dim br As BinaryReader

        Sub Main(args As String())
            br = New BinaryReader(File.OpenRead(args(0)))
            br.BaseStream.Position = br.BaseStream.Length - 12
            Dim metaTableSize As Integer = br.ReadInt32()
            Dim metaTableStart As Integer = br.ReadInt32()
            br.BaseStream.Position = br.BaseStream.Length - (12 + metaTableSize)

            Dim files As New List(Of MetaTable)()
            While br.BaseStream.Position < br.BaseStream.Length - 12
                files.Add(ReadMeta())
            End While

            For Each file As MetaTable In files
                If file.start = -1 Then
                    Directory.CreateDirectory(Path.GetDirectoryName(args(0)) & "\" & file.name)
                    Continue For
                End If

                br.BaseStream.Position = file.start
                Dim bw As New BinaryWriter(File.OpenWrite(Path.GetDirectoryName(args(0)) & "\" & file.name))
                bw.Write(br.ReadBytes(file.size))
                bw.Close()
            Next
        End Sub

        Public Function ReadMeta() As MetaTable
            Dim size As Integer = br.ReadInt32()
            Return New MetaTable() With {
                .start = br.ReadInt32(),
                .size = br.ReadInt32(),
                .unknown2 = br.ReadInt32(),
                .name = New String(Encoding.GetEncoding("UTF-16").GetChars(br.ReadBytes(size - 16)))
            }
        End Function

        Public Structure MetaTable
            Public start As Integer
            Public size As Integer
            Public unknown2 As Integer
            Public name As String
        End Structure
    End Module


