'https://github.com/odasm/AxpPackerTLBB/blob/master/tests/AxpUnpackV1/AXPFile.h
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
            Dim signature as String = New String(br.ReadChars(4)) ' Offset = 0, Length = 4
            Dim unknow1 as Int32 = br.ReadInt32 ' Offset = 8, Length = 4
            Dim unknow2 as Int32 = br.ReadInt32 ' Offset = 12, Length = 4
            Dim nHashTableOffset as Int32 = br.ReadInt32 ' Offset = 16, Length = 4
            Dim nIndexTableOffset as Int32 = br.ReadInt32 ' Offset = 20, Length = 4
            Dim nFileCount as Int32 = br.ReadInt32 ' Offset = 24, Length = 4
            Dim nSizeOfIndexTable as Int32 = br.ReadInt32 ' Offset = 28, Length = 4
            Dim nDataOffset as Int32 ' Offset = 32, Length = 4
            Dim unknow3 as Int32 = br.ReadInt32 ' Offset = 36, Length = 4
            Dim unknow4 as Int32 = br.ReadInt32 ' Offset = 40, Length = 4
            br.BaseStream.Position = 44
            Dim subtables as New List(of TableData)()
            Dim subfiles as New Lisr(of FileData)()
            
            For i as Int32 = 0 To FileCount - 1
             subfiles.Add(New FileData)
            Next
        
            p = Path.GetDirectoryName(input) & "\" & Path.GetFileNameWithoutExtension(input)
            Directory.CreateDirectory(p)
             
             For fd as FileData in subfiles
                br.BaseStream.Position = fd.offset
                Using bw As New BinaryWriter(File.Create(p & "//" & fd.id))
                    bw.Write(br.ReadBytes(fd.size))
                End Using
              Next
            Console.WriteLine("unpack done!!!")
        End If
            Console.ReadLine()
        End Sub

Class TableData
    Public hashA as String
    Public hashB as String
    Public isExit as String
    Public Sub New()
        hashA = New String(br.ReadChars(4))
        hashB = New String(br.ReadChars(4))
        isExit = New String(br.ReadChars(4))
        End Sub
    End Class

 Class FileData
     Public offset as Int32
      Public size as Int32
      Public id as Int32
      
      Public Sub New()
        offset = br.ReadInt32
        size = br.ReadInt32
        id = br.ReadInt32
        End Sub
  End Class  

End Module     
