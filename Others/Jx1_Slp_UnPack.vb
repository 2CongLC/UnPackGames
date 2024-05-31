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
            subSprOffs.Add(New SprOffs) ' Length = 8

            br.BaseStream.Position += nFrames * 8

            Dim subKPal16 as New List(Of KPal16)()
            For Each k24 as KPal24 in subKPal24
              While colors > 0
                Dim red = k24.Red >> 4
                Dim green = k24.Green >> 4
                Dim blue = k24.Blue >> 4
                subKPal16.Add(New KPal16() With {
                            .Red = Red << 8,
                            .Green = green << 8,
                            .Blue = blue << 8 }   
               End While
            Next

            p = Path.GetDirectoryName(input) & "\" & Path.GetFileNameWithoutExtension(input)
            Directory.CreateDirectory(p)

            For Each fd as SprOffs in subSprOffs
        
             br.BaseStream.Position = fd.offset
             
             Using bw As New BinaryWriter(File.Create(p & "//" & fd.offset))
                 bw.Write(br.ReadBytes(fd.size))
             End Using
            Next 
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

  Class KPal16
        Dim Red as Byte ' Length = 1
        Dim Green as Byte ' Length  = 1
        Dim Blue as Byte ' Length = 1
   End Class
            
  Class SprOffs
        Dim offset as UInt32 ' Length = 4
        Dim size as UInt32 ' Length = 4
        Public Sub New()
            offset = br.ReadUInt32
            size = br.ReadUInt32
        End Sub
   End Class

   Class SprFrame
        Dim width as UInt16
        Dim heigth as UInt16
        Dim offsetX as UInt16
        Dim offsetY as UInt16
        Dim sprite as Byte
        Public Sub New()
            width = br.ReadUInt16
            heigth = br.ReadUInt16
            offsetX = br.ReadUInt16
            offsetY = br.ReadUInt16
            sprite = br.ReadByte
        End Sub
      End Class
                
        
       
End Module     
