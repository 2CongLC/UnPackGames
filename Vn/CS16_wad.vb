' https://github.com/yuraj11/HL-Texture-Tools/blob/master/HL%20Texture%20Tools/HLTools/WAD3Loader.cs

Private br as BinaryReader
Private des as String
Private source as String
Private buffer as Byte()
Private subfiles as New List(Of FileData)()

des = Path.GetDirectory(source) + "\" + Path.GetFileNameWithOutExtension(source) + "\"
br = New BinaryReader(File.OpenRead(source))

Dim sign as String = New String(br.ReadChars(4))
Dim count_FileData as Int32 = br.ReadInt32
Dim offset_FileData as Int32 = br.ReadInt32

br.BaseStream.Position = offset_FileData
For i as Int32 = 0 To count_FileData - 1
 subfiles.Add(New FileData)
Next

Directory.Create(des)

For Each fd as FileData in subfiles

Next




==========

Class FileData
 Public offset as UInt32 = br.ReadUInt32
 Public sizeCompressed as UInt32 = br.ReadUInt32
 Public size as UInt32 = br.ReadUInt32
 Public types as Byte = br.ReadByte
 Public compression as Byte = br.ReadByte
 Public name as String
 Public Sub New()
  br.BaseStream.Seek(2, SeekOrigin.Current)
  name = New String(br.ReadChars(16)).TrimEnd(ChrW(0))
 End Sub
End Class
