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
            Console.WriteLine("signature : {0}", signature)
            Dim count As Int32 = br.ReadInt32
            Console.WriteLine("count : {0}", count)
            Dim index_offset as Int32 = br.ReadInt32
            Console.WriteLine("Index Offset : {0}",index_offset)
            Dim data_offset as Int32 = br.ReadInt32 ' Size of Header
            Console.WriteLine("Data Offset : {0}", data_offset)
            Dim crc32 as Int32 = br.ReadInt32
            Console.WriteLine("Crc32 : {0},crc32)
            Dim reserved as String = Encoding.GetEncoding("gbk").GetString(br.ReadBytes(12))
            Console.WriteLine("Reserved : {0}", reserved)
                
            Dim subfiles As New List(Of FileData)()
            For i As Int32 = 0 To count - 1
                subfiles.Add(
                    New FileData
                )
            Next  
            p = Path.GetDirectoryName(input) & "//" & Path.GetFileNameWithoutExtension(input)
            Directory.CreateDirectory(p)
        
            For Each f As FileData In subfiles

                Console.WriteLine("File ID : {0} - File Offset : {1} - File Size : {2} - File Compress Size : {3} ", f.id, f.offset, f.size, f.compress_size)
                ' Using bw As New BinaryWriter(File.Create(p & "//" & f.id))
                ' bw.Write(br.ReadBytes(f.compress_size))
                'End Using
            Next


        End If
        Console.ReadLine()
    End Sub
   
    Class FileData
        Public id As Int32
        Public offset As Int32
        Public size As Int32
        Public compress_size As Int32
        Public Sub New()
            id = br.ReadInt32
            offset = br.ReadInt32
            size = br.ReadInt32
            compress_size = br.ReadInt32
        End Sub
    End Class

End Module
