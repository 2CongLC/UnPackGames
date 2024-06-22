
Private ms as New MemoryStream()
===========

Dim unknow as Int32 = br.ReadInt32
Dim count as Int32 = br.ReadInt32
Dim offset as Int32 = br.ReadInt32
Dim unknow1 as Int32 = br.ReadInt32
Dim unknow2 as Int32 = br.ReadInt32
Dim sizeName as Int32 = br.ReadInt32
Dim unknow3 as Int16 = br.ReadInt16

Using dfs as New DeflateStream(New MemoryStream(br.ReadBytes(unsize - 2)),CompressionMode.Decompress)
  dfs.copyto(ms)
End Using

Dim subfiles as New List(Of FileData)()
For i as Int32 = 0 To count - 1
 subfiles.Add(New FileData)
Next

br.BaseStream.Position = offset

