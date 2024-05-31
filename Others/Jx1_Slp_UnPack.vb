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

            br.BaseStream.Position += 26

            Dim subKPal24 as New List(of KPal24)()
            subKPal24.Add(New KPal24) ' Length = 3

            br.BaseStream.Position = colors * 3

            Dim subSprOffs as New List(Of SprOffs)()
            subSprOffs.Add(New SprOffs) ' Length = 4

            br.BaseStream.Position += nFrames * 4
            
        End If
            Console.ReadLine()
        End Sub

  Class KPal24
        Dim Red as Byte ' Length = 1
        Dim Green as Byte ' Length = 1
        Dim Blue as Byte ' Length = 1
        Public Sub New()
            Red = br.ReadByte
            Green = br.ReadByte
            Blue = br.ReadByte
        End Sub         
  End Class 

  Class SprOffs
        Dim offset as UInt16 ' Length = 2
        Dim size as UInt16 ' Length = 2
        Public Sub New()
            offset = br.ReadUInt16
            size = br.ReadUInt16
        End Sub
   End Class

   

    

End Module     
