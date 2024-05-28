'https://www.clbgamesvn.com/diendan/showthread.php?t=314825
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
            Dim count as Int32 = br.ReadInt32 ' Offset = 4, Length = 4
            Dim index as Int32 = br.ReadInt32 ' Offset = 8, Length = 4
            Dim data as Int32 = br.ReadInt32 ' Offset = 12, Length = 4
            Dim crc32 as Int32 = br.ReadInt32 ' Offset = 16, Length = 4
            Dim reserved as String = New String(br.ReadChars(12)) 'Offset = 20, Length = 12

            br.BaseStream.Position = 32

            Dim subfiles as New List(of FileData)()
            For i as Int32 = 0 To count - 1
              subfiles.Add(New FileData)
            Next
            


        
        End If
            Console.ReadLine()
        End Sub

  Class FileData
      Public id as Int32 'Length = 4
      Public offset as Int32 'Length = 4
      Public size as Int32 'Length = 4
      Public compressed as  Byte() 'Length = 3
      Public isCompress as byte 'Length = 1
      Public Sub New()
        id = br.ReadInt32
        offset = br.ReadInt32
        size = br.ReadInt32
        compressed = br.ReadBytes(3)
        isCompress = br.ReadByte
        br.BaseStream.Position +=16
      End sub
  End Class  

End Module     
