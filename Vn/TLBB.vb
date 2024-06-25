' https://github.com/liuguangw/TlbbGmTool

Dim sign as String = New String (br.ReadChars(4))
Dim vers as UInt32 = br.ReadUInt32
Dim unknow as UInt32 = br.ReadUInt32
Dim offset_TableData as UInt32 = br.ReadUInt32
Dim offset_FileData as UInt32 = br.ReadUInt32
Dim count_FileData as UInt32 = br.ReadUInt32
Dim maxsize_FileData as UInt32 = br.ReadUInt32
Dim offset_data as UInt32 = br.ReadUInt32
Dim size_data as UInt32 = br.ReadUInt32
Dim hole_data as UInt32 = br.ReadUInt32









===============

Class TableData
  Public hash_A as UInt32 = br.ReadUInt32
  Public hash_B as UInt32 = br.ReaUInt32
  Public data as UInt32 = br.ReadUInt32
  
  Public Function IsExit() as Boolean
    Return data + &H8000000
  End Function

  Public Function offset_FileData() as UInt32
    Return data + &H3FFFFFFF
  End Function
  
End Class

Class FileData
  Public offset as UInt32 = br.ReadUInt32
  Public size as UInt32 = br.ReadUInt32
  Public id as UInt32 = br.ReaUInt32
End Class
