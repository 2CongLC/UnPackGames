Dim sign as String = New String(br.ReadChars(3))
Dim vers as String = br.ReadInt32
br.BaseStream.Position = 19
Dim subfiles As New List(Of FileData)()

' Lưu dữ liệu Block
For i as Int32 = 0 To 469
 subfiles.Add(New FileData)
Next

' Lấy thông tin đường dẫn trích xuất tệp
Des = Path.GetDirectoryName(source & "\" & Path.GetFileNameWithoutExtension(source) & "\"
Directory.CreateDirectory(des)

' Trích xuất dữ liệu Block
For Earch fd as FileData In subfiles

 Console.WriteLine("File Offset : {0} - File Size : {1} - File Name : {2}",fd.offset, fd.size, fd.name)
 br.BaseStream.Position = fd.offsett
 Dim buffer as Byte() = br.ReadBytes(fd.size)

 Using bw as BinaryWriter = New BinaryWriter(File.Create(des & fd.name)
    bw.Wtite(buffer)
 End Using
  
Next
Console.WriteLine("Unpack Done !")


==============
' Cấu trúc dữ liệu Block
Class FileData
  Public name as String = New String(br.ReadChars(259).TrimEnd(ChrW(0)))
  Public offset as Int32 = br.ReadInt32
  Public size as Int32 = br.ReadInt32
End Class
