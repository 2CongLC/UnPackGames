br.BaseStream.Position = 6
Dim offset as Int64 = br.ReadInt64
br.BaseStream.Position = offset
br.BaseStream.Position += 22
Dim count as Int32 = br.ReadInt32
Dim subfiles as New List(Of FileData)()

For i as Int32 = 0 To count - 1
 subfiles.Add(New FileData)
 Dim unknow as Int64 = br.ReadInt64
Next

des = Path.GetDirectoryName(source) + "\" + Path.GetFileNameWithoutExtension(source) + "\"
Directory.CreateDirectory(des)

Dim n as Int32 = 0
For Each fd as FileData in subfiles
 Console.WriteLine("File Offset : {0} - File Size : {1}", fd.offset, fd.size)
 br.BaseStream.Position = fd.offset
 Dim buffer as Byte() = br.ReadBytes(fd.size)
 Using bw as New BinaryWriter(File.Create(des + n)
  br.Write(buffer)
 End Using

 br = New BinaryReader(File.OpenRead(des + n)
 Dim sign as String = New String(Encoding.UTF7.GetString(br.ReadBytes(4)))
 If sign = "Game" Or sign = ";Game" Then
    sign += New String(Encoding.UTF7.GetString(br.ReadBytes(9)))
 End If
 br.close()

 If sign = "\u0089PNG" Then
    File.Move(des + "\" + n, des + "\" + n + ".png")
 Elseif sign = "DDS " Then
    File.Move(des + "\" + n, des + "\" + n + ".dds")
 Elseif sign = "Gamebryo File" Then
    File.Move(des + "\" + n, des + "\" + n + ".nif")
 Elseif sign = ";Gamebryo KFM" Then
    File.Move(des + "\" + n, des + "\" + n + ".kfm")
 Else
    File.Move(des + "\" + n, des + "\" + n + ".unknow")
 End If
 n +=1
Next
Console.WriteLine("Unpack done !!!")

=========
' cấu trúc dữ liệu block
Class FileData
 Public unknown As Byte = br.ReadByte 'usually 0x4000. Sometimes 0x2000.
 Public offset As Int64 = br.ReadInt64
 Public size As Int32 = br.ReadInt32
End Class  
