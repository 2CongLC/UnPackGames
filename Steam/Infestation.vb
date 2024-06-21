' Written for the Infestation series.
' Infestation: Battle Royale https://store.steampowered.com/app/1240290
' Infestation: Survivor Stories https://store.steampowered.com/app/226700
' Infestation: The New Z https://store.steampowered.com/app/555570
Imports System.IO
Imports System.IO.Compression


br = New BinaryReader(File.OpenRead(source))
Dim des as String = Path.GetDirectoryName(source) + "\"

br.BaseStream.Position = 26
Dim ms As MemoryStream = New MemoryStream()
Using ds As DeflateStream = New DeflateStream(New MemoryStream(br.ReadBytes(CInt(br.BaseStream.Length - 26))), CompressionMode.Decompress)
    ds.CopyTo(ms)
End Using

br = New BinaryReader(ms)
br.BaseStream.Position = 0
Dim subfiles as New List(Of FileData)()
While br.BaseStream.Position < br.BaseStream.Length
 subfiles.Add(New FileData)
 Dim unknow as Byte() = br.ReadBytes(14)
End While

For Each fd as FileData In subfiles
 If fd.container < 9 Then
  br = New BinaryReader(File.OpenRead(des + "WZ_0" + (fd.container + 1) + ".bin"))
 Else
   br = New BinaryReader(File.OpenRead(des + "WZ_" + (fd.container + 1) + ".bin"))
 End If
 Console.WriteLine("File Offset : {0} - File Size : {1} - File Name : {2},fd.offset, fd.size, fd.name)
 br.BaseStream.Position = fd.offset
 Dim buffer as Byte() 
 Directory.CreateDirectory(des + Path.GetFileNameWithoutExtension(source) + "\" + Path.GetDirectoryName(fd.name))
 
 If fd.iscompressed = 2 Then
    buffer = br.ReadBytes(sizeCompressed)
    Dim fs as  FileStream = File.Create(des + Path.GetFileNameWithoutExtension(source) + "\" + fd.name)
    Dim unknow1 as Int16 = br.ReadInt16
    Using dfs as New DeflateStream(New MemoryStream(buffer), CompressionMode.Decompress)
      dfs.copyto(fs)
    End Using
    fs.close()
  Else
    buffer = br.ReadBytes(sizeUncompressed)
    Using bw as New BinaryWriter(des + Path.GetFileNameWithoutExtension(source) + "\" + fd.name)
      bw.Write(buffer)
    End Using
  End If
Next
Console.WriteLine("Unpack Done !!!")

============
' Cấu trúc dữ liệu block

Class FileData
 Public name As String = New String(br.ReadChars(260)).TrimEnd(ChrW(0))
 Public isCompressed As Byte = br.ReadByte()
 Public container As Byte = br.ReadByte()
 Public offset As Integer = br.ReadInt32() + 4
 Public sizeUncompressed As Integer = br.ReadInt32()
 Public sizeCompressed As Integer = br.ReadInt32()
 Public checksum As Single = br.ReadSingle()
 Public unknown As Integer = br.ReadInt32()
End Class
