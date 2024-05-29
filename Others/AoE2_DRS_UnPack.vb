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
             p = Path.GetDirectoryName(input) & "\" & Path.GetFileNameWithoutExtension(input)
             Directory.CreateDirectory(p)
             For fd as FileData in subfiles
                br.BaseStream.Position = fd.offset
                Dim buffer as Byte() = br.ReadBytes(fd.size)
                Dim ext as String = GetExtension(buffer)
                Using bw As New BinaryWriter(File.Create(p & "//" & fd.name & ext))
                    bw.Write(buffer)
                End Using
              Next
            Console.WriteLine("unpack done!!!")
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

Private ReadOnly DataFormats As New Dictionary(Of String, Func(Of String, Boolean))() From {
        {".slp", AddressOf IsSlp},
        {".wav", AddressOf IsWav}
    }

Public Function IsWav(ByVal data As Byte()) As Boolean
        Return (data(0) = &H52 AndAlso data(1) = &H49 AndAlso data(2) = &H46 AndAlso data(3) = &H46) _
            AndAlso (data(8) = &H57 AndAlso data(9) = &H41 AndAlso data(10) = &H56 AndAlso data(11) = &H45)
    End Function

Public Function IsSlp(ByVal data As Byte()) As Boolean
        Dim header as Byte() = data.Take(4).ToArray()
        Dim s as String = Encoding.ASCII.GetString(header)
        Return (s = "2.0N") OrElse (s = "3.0") OrElse (s = "4.0X") OrElse (s = "4.1X") OrElse (s = "4.2P") 
    End Function

Public Function GetExtension(ByVal data As Byte()) As String
       
     For Each binFmt In DataFormats
        Try
           If binFmt.Value(data) Then
              Return binFmt.Key
            End If
        Catch
        End Try
        Next
        Return ".bina"
 End Function

End Module     
