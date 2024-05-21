

Public Class Jx1UnPack


Class Header
Public signature as String ' Offset = 0, Length = 4
Public count as ULong ' Offset = 4, Length = 8
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
