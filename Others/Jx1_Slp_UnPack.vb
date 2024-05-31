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
            Dim width as UInt16 = br.ReadUInt16 ' Offset = 4, Length = 2
            Dim height as UInt16 = br.ReadUInt16 ' Offset = 6, Length = 2
            Dim centerX as UInt16 = br.ReadUInt16 ' Offset = 8, Length = 2
            Dim centerY as UInt16 = br.ReadUInt16 ' Offset = 10, Length = 2
            Dim nFrames as UInt16 = br.ReadUInt16 ' Offset = 12, Length = 2
            Dim colors as UInt16 = br.ReadUInt16 ' Offset = 14, Length = 2
            Dim directions as UInt16 = br.ReadUInt16 ' Offset = 16, Length = 2
            Dim interval as UInt16 = br.ReadUInt16 ' Offset = 18, Length = 2
            Dim reserved as Byte() = br.ReadBytes(6) ' Offset = 20, Length = 6

            br.BaseStream.Position = 26

            Dim subfiles as New List(of FileData)()
            For i as UInt16 = 0 To nFrames - 1
              subfiles.Add(New FileData)
            Next

            p = Path.GetDirectoryName(input) & "\" & Path.GetFileNameWithoutExtension(input)
            Directory.CreateDirectory(p)

            For Each fd as FileData in subfiles
             br.BaseStream.Position = fd.offset
             
             Using bw As New BinaryWriter(File.Create(p & "//" & fd.id))
                 bw.Write(br.ReadBytes(fd.size))
             End Using
            Next
            Console.WriteLine("unpack done!!!")
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
      End sub
  End Class  

End Module     
