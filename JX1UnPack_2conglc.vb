Imports System
Imports System.IO

    Class Program
        Shared Sub Main(ByVal args As String())
            Dim br As New BinaryReader(File.OpenRead(args(0)))
If New String(System.Text.Encoding.ASCII.GetString(br.ReadBytes(4)) <> "PACK" Then
                Throw New Exception("This is not an Jx1 Online")
            End If
Dim fileCount Uint64 = br.ReadUint64()
     Dim subtables As New list(of TableData)()
      For i As UInt64 = 0 To fileCount - 1
      subtables.Add(New TableData() With {
            .index_offset = br.ReadUint64(),
            .data_offset = br.ReadUint64(),
            .crc32 = br.ReadUint64(),
            .reserved = br.ReadChars(12)})
      Next
 Dim subfiles as New List(of FileData)()
 For Each td as TableData In Subtables
  br.BaseStream.Position = td.index_offset
    subfiles.Add(New FileData() With {
          .id = br.ReadUint64(),
          .offset = br.ReadUint64(),
          .size = br.ReadInt64(),
          .compress_size = br.ReadInt64()}) 
          Next
      Dim path As String = Path.GetDirectoryName(args(0)) & "//" & Path.GetFileNameWithoutExtension(args(0))
      Directory.CreateDirectory(path)
        For Each f as FileData in subfiles
           br.BaseStream.Position = f.offset
            using bw As New BinaryWriter(File.Create(path & "//" & f.id))
                bw.Write(br.ReadBytes(f.size))
            end using    
         Next   
  End Sub

  
Class TableData
'Public signature as String ' Offset = 0, Length = 4
'Public count as ULong ' Offset = 4, Length = 8
Public index_offset as ULong  ' Offset = 12, Length = 8  
Public data_offset as Ulong ' Offset = 20, Length = 8
Public crc32 as ULong ' Offet = 28, Length = 8
Public reserved as String 'Offset = 36, Length = 12   
End Class

Class FileData
Public id as ULong
Public offset as ULong
Public size as Long
Public compress_size as Long     
End Class 
  
End Class
