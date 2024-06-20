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

des = Path.GetDirectoryName(source) + "/"

For Each fd as FileData In subfiles
 If fd.Isfolder = 1 Then
  Directory.CreateDirectory(des + fd.path)
 Else

 End If

Next
