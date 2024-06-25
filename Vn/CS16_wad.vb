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
