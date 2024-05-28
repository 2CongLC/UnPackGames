'https://github.com/SFTtech/openage/blob/master/doc/media/drs-files.md
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
            Dim signature as String = New String(br.ReadChars(40)) ' Offset = 0, Length = 40
            Dim version as Int32 = br.ReadInt32 ' Offset = 40, Length = 4
            Dim description as String = New String(br.ReadChars(12)) ' Offset = 44, Length = 12
            Dim count as Int32 = br.ReadInt32 ' Offset = 56, Length = 4
            Dim indexOffset as Int32 = br.ReadInt32 ' Offset = 60, Length = 4
            br.BaseStream.Position = 64
            Dim subtables as New List(of TableData)()
            For i as Int32 = 0 To count - 1
              subtables.Add(New TableData)
            Next
            Dim subfiles as New List(of FileData)()
            Dim name as String = Nothing
            For td as TableData in subtables
               name = td.name
               br.BaseStream.Position = td.offset
               For j as Int32 = 0 To td.count - 1
                subfiles.Add(New FileData)
               Next
             Next

            
            


        
        End If
            Console.ReadLine()
        End Sub

Class TableData
    Public name as String
    Public offset as Int32
    Public count as Int32
    Public Sub New()
        name = New String(br.ReadChars(4))
        offset = br.ReadInt32
        count = br.ReadInt32
        End Sub
    End Class

 Class FileData
     Public id as Int32
      Public offset as Int32
      Public size as Int32
      
      Public Sub New()
        id = br.ReadInt32
        offset = br.ReadInt32
        size = br.ReadInt32
        End Sub
  End Class  

End Module     
