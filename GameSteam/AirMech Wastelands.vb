
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
des = Path.GetDirectoryName(source) + "\" + Path.GetFileNameWithoutExtension(source)
For Each fd as FileData in subfiles
 Console.WriteLine("File Offset : {0} - File Size : {1} - File Name : {2}", fd.offset, fd.size, fd.name)
 br.BaseStream.Position = fd.offset
 Dim buffer as Byte()
 Directory.CreateDirectory(des + "\" + Path.GetDirectoryName(fd.name))
 Dim fs as FileStream = File.Create(des + "\" + fd.name)
 If fd.sizeCompressed = fd.sizeUncompressed Then
  buffer = br.ReadBytes(fd.sizeUncompressed)
  Using bw as New BinaryWriter(fs)
   bw.Write(buffer)
  End Using
 Else
  buffer = br.ReadBytes(fd.sizeCompressed - 2)
  Dim unknow4 as Int16 = br.ReadInt16
  Using dfs as New DeflateStream(New MemoryStream(buffer),CompressionMode.Decompress)
    dfs.copyto(fs)
  End Using
 End If
 fs.close()
  
Next
br.close()
Console.WriteLine("Unpack Done !!!")

================
' cấu trúc dữ liệu của block
Class FileData
 Public sizeCompressed As Int32 = br.ReadInt32()
 Public offset As Int32 = br.ReadInt32()
 Public sizeUncompressed As Int32 = br.ReadInt32()
 Public unknown As Single = br.ReadSingle()
 Public name as String = getname(br.ReadInt32)
End Class

Private Function getname(Byval offset As Int32) As String
    ms.Position = offset
    Dim _name As String = ""
    Dim x As Byte = CByte(ms.ReadByte)
    While x <> 0
        _name &= CChar(x)
        x = CByte(ms.ReadByte)
    End While
    Return _name
End Function



