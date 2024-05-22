Imports System
Imports System.Collections
Imports System.IO
Imports System.IO.Compression
Imports System.Runtime
Imports System.Text
Imports System.Text.RegularExpressions


Module Program

    Public br As BinaryReader
    Public input As String
    Sub Main(args As String())
        If args.Count = 0 Then
            Console.WriteLine("Tool UnPack - 2CongLC.vn :: 2024")
        Else
            input = args(0)
        End If
        Dim p As String = Nothing
        If IO.File.Exists(input) Then

            br = New BinaryReader(File.OpenRead(input))
            Dim signature As String = Encoding.ASCII.GetString(br.ReadBytes(4))
            Console.Writeline("signature : {0},signature)
            Dim unknow1 as Int32 = br.ReadInt32
            Dim unknow2 as Int32 = br.ReadInt32
            Dim nHashTableOffset as Int32 = br.ReadIn32
            Dim nIndexTableOffset as Int32 = br.ReadInt32
            Dim nFileCount as Int32 = br.ReadInt32
            Console.WriteLine("Total Files : {0},nFileCount)
            Dim nSizeOfIndexTable as Int32 = br.ReadInt32
            Dim nDataOffset as Int32 = br.ReadInt32
            Dim unknow3 as Int32 = br.ReadInt32
            Dim unknow4 as Int32 = br.ReadInt32
            
            Dim subtables As New List(Of TableData)()
            For i As Int32 = 0 To nSizeOfIndexTable - 1
                subtables.Add(
                    New TableData
                )
            Next

            Dim subfiles As New List(Of FileData)()
            For j As Int32 = 0 To nFileCount - 1
                subfiles.Add(
                        New FileData
                )
            Next
                
            p = Path.GetDirectoryName(input) & "//" & Path.GetFileNameWithoutExtension(input)
            Directory.CreateDirectory(p & "//" & d)
            For Each f As FileData In subfiles

                Console.WriteLine("File Offset : {0} - File Size : {1} - File Flag : {2}", f.offset, f.size, f.flag)
                Using bw As New BinaryWriter(File.Create(p & "//" & d & "//" & f.id))
                    bw.Write(br.ReadBytes(f.size))
                End Using
            Next
            Console.WriteLine("UnPak Done !!!")

        End If
        Console.ReadLine()
    End Sub


    Class TableData

        Public nHashA As String
        Public nHashB As String
        Public nExits As String

        Public Sub New()
            HashA = Encoding.GetEncoding("us-ascii").GetString(br.ReadBytes(4))
            HashB = Encoding.GetEncoding("us-ascii").GetString(br.ReadBytes(4))
            Exits = Encoding.GetEncoding("us-ascii").GetString(br.ReadBytes(4))
        End Sub
    End Class

    Class FileData
        Public offset As Int32
        Public size As Int32
        Public flag As Int32

        Public Sub New()
            offset = br.ReadInt32
            size = br.ReadInt32
            flag = br.ReadInt32
        End Sub
    End Class

End Module
