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
            Dim copyrigth As String = Encoding.ASCII.GetString(br.ReadBytes(40))
            Console.WriteLine("signature : {0}", copyrigth)
            Dim version As String = Encoding.ASCII.GetString(br.ReadBytes(4))
            Console.WriteLine("version : {0}", version)
            Dim types As String = Encoding.ASCII.GetString(br.ReadBytes(12))
            Console.WriteLine("File Type : {0}", types)
            Dim tablecount As Int32 = br.ReadInt32
            Console.WriteLine("Table Count : {0}", tablecount)
            Dim fileoffset As Int32 = br.ReadInt32
            Console.WriteLine("File Offset : {0}", fileoffset)

            Dim subtables As New List(Of TableData)()
            For i As Int32 = 0 To tablecount - 1
                subtables.Add(
                    New TableData
                )
            Next

            Dim subfiles As New List(Of FileData)()
            Dim d As String = Nothing
            Console.WriteLine(" --------------- Table Format --------------------")
            Console.WriteLine()

            For Each td As TableData In subtables
                Console.WriteLine("Extension : {0} -  File Offset : {1} - Num Of Files : {2} ", td.file_extension, td.file_info_offset, td.num_files)
                For j As Int32 = 0 To td.num_files - 1
                    subfiles.Add(
                         New FileData
                     )
                Next

                d = td.file_extension
                p = Path.GetDirectoryName(input) & "//" & Path.GetFileNameWithoutExtension(input)
                Directory.CreateDirectory(p & "//" & d)
            Next

            Console.WriteLine(" --------------- File List --------------------")
            Console.WriteLine()


            For Each f As FileData In subfiles

                Console.WriteLine("File ID : {0} - File Offset : {1} - File Size : {2}", f.id, f.offset, f.size)
                Using bw As New BinaryWriter(File.Create(p & "//" & d & "//" & f.id))
                    bw.Write(br.ReadBytes(f.size))
                End Using
            Next
            Console.WriteLine("UnPak Done !!!")

        End If
        Console.ReadLine()
    End Sub


    Class TableData

      Public coffCabStart As UInteger ' offset of the first CFDATA block in this folder
      Public cCFData As UShort ' number of CFDATA blocks in this folder
      Public typeCompress As UShort ' compression type indicator
      Public abReserve As Byte() ' (optional) per-folder reserved area

        Public Sub New()
            coffCabStart = br.ReadUInt32
            cCFData = br.ReadUInt32
            typeCompress = br.ReadUInt16
            'abReserve 
        End Sub
    End Class

    Class FileData
        Public id As Int32
        Public offset As Int32
        Public size As Int32

        Public Sub New()
            id = br.ReadInt32
            offset = br.ReadInt32
            size = br.ReadInt32
        End Sub
    End Class

End Module
