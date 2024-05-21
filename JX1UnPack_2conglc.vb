Imports System
Imports System.IO

    Class Program
        Shared Sub Main(ByVal args As String())
            Dim br As New BinaryReader(File.OpenRead(args(0)))
If New String(System.Text.Encoding.ASCII.GetString(br.ReadBytes(4)) <> "PACK" Then
                Throw New Exception("This is not an Jx1 Online")
            End If
Dim fileCount Uint64 = br.ReadUint64()
     Dim subfiles As New list(of header)
      For i As UInt64 = 0 To fileCount - 1
      subfiles.Add(New Subfile() With {
            .index_offset = br.ReadUint64(),
            .data_offset = br.ReadUint64(),
            .crc32 = br.ReadUint64(),
            .reserved = br.ReadChars(12)})
      Next
  
  
  End Sub

  
Class Header
'Public signature as String ' Offset = 0, Length = 4
'Public count as ULong ' Offset = 4, Length = 8
Public index_offset as ULong  ' Offset = 12, Length = 8  
Public data_offset as Ulong ' Offset = 20, Length = 8
Public crc32 as ULong ' Offet = 28, Length = 8
Public reserved as String 'Offset = 36, Length = 12   
End Class

Class IndexItems
Public id as ULong
Public offset as ULong
Public size as Long
Public compress_size as Long     
End Class 
  
End Class
