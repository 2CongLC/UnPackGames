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
            Dim signature as String = New String(br.ReadChars(4))
            Dim count as Int32 = br.ReadInt32
            Dim subfiles as New List(of FileData)()
            For i as Int32 = 0 To count - 1
              subfiles.Add(New FileData)
            Next
            


        
        End If
            Console.ReadLine()
        End Sub

  Class FileData
      Public id as Int32
      Public offset as Int32
      Public size as Int32
      Public compressSize as Byte()
      Public isCompress as Byte
      Public Sub New()
        id = br.ReadInt32
        offset = br.ReadInt32
        size = br.ReadInt32
        compressSize = br.Readbytes(br.ReadInt32)
        isCompress = br.ReadByte
  End Class  

End Module     
