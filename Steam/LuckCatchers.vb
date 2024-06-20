
'Dim sign as String = New String(br.ReadChars())
'Dim vers as Int32 = br.ReadInt32
br.BaseStream.Position = 8

Dim subfiles as New List(Of FileData)()
subfiles.Add(New FileData)
For i as Int32 = 0 To subfiles(0).size - 2
subfiles.Add(New FileData)
Next

Dim unknow as Int32 = br.ReadInt32
Dim dataoffs as Int64 = br.BaseStream.Position

Des = Path.GetDirectoryName(source & "\" & Path.GetFileNameWithoutExtension(source) & "\"

For Each fd as FileData In subfiles

 Console.WriteLine("File Offset : {0} - File Size : {1} - File Name : {2}",fd.offset, fd.size, fd._path)

 If fd.Isfolder = 1 Then
  Directory.CreateDirectory(des + fd.path)
 Else
  br.BaseStream.Position = fd.offset + dataoffs
  Dim buffer as Byte() = br.ReadBytes(fd.size)

  Using bw as BinaryWriter = New BinaryWriter(File.Create(des & fd._path))
   bw.Write(buffer)
  End Using
 
 End If

Next
Console.WriteLine("Unpack Done!!!")

=============
' cấu trúc dữ liệu block
Class FileData
 Public size as Int32 = br.ReadInt32
 Public isFolder as Byte = br.ReadByte
 Public name as String = New String(br.ReadChars(br.ReadInt32)).TrimEnd(ChrW(0))
 Public _path as String = New String(br.ReadChars(br.ReadInt32)).TrimEnd(ChrW(0))
 Public offset as Int32 = br.ReadInt32
End Class
