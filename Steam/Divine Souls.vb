Dim des as String = Path.GetDirectoryName(source) + "/" + Path.GetFileNameWithoutExtension(source)
Dim dfp as new BinaryReader(File.OpenRead(des + ".dfp")
Dim dfh as New BinaryReader(File.OpenRead(des + ".dfh")

Dim unknow as Int32 = br.ReadInt32
Dim count as Int32 = br.ReadInt32
Dim unknow1 as Int32 = br.ReadInt32

Directory.CreateDirectory(des)

For i as Int32 = 0 To count - 1
 Dim size as Int32 = dfh.ReadInt32
 Dim unknow2 as Int32 = dfh.ReadInt32
 Dim offset as Int32 = dfh.ReadInt32
 Console.WriteLine("File Offset : {0} - File Size : {1}",offset, size)
 dfh.BaseStream.Position = offset
 Dim buffer as Byte() = br.ReadBytes(size)
    
 Using bw as New BinaryWriter(File.Create(des & "/" & i)
     bw.Write(buffer)
 End Using

 Dim br as New BinaryReader(File.OpenRead(des & "/" & i)
 Dim sign as String = New String(br.ReadChars(4))
 If sign = "Game" Then
    sign += New String(br.ReadChars(9))
 br.close()

If sign = "DDS " Then
    File.Move(des + "/" + i, des + "/" + i + ".dds")
ElseIf sign = "Gamebryo File" Then
    File.Move(des + "/" + i, des + "/" + i + ".nif")

End If
Next
Console.WriteLine("Unpack done!")
