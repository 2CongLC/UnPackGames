
'Written for games in the eSofnet engine.
'Legend of Edda: Pegasus https://store.steampowered.com/app/2241570/
'Luna Online: Reborn https://store.steampowered.com/app/457590/
'Super Mecha Champions https://store.steampowered.com/app/1368910/

Imports System.IO

    Class Program
        Shared Sub Main(ByVal args As String())
            Dim br As New BinaryReader(File.OpenRead(args(0)))

            br.ReadInt32()
            Dim filecount As Integer = br.ReadInt32()
            br.BaseStream.Position = &H5C
            Dim path As String = Path.GetDirectoryName(args(0)) & "//" & Path.GetFileNameWithoutExtension(args(0))
            Directory.CreateDirectory(path)
            For i As Integer = 0 To filecount - 1
                Dim size As Integer = br.ReadInt32()
                Dim fileSize As Integer = br.ReadInt32()
                Dim nameSize As Integer = br.ReadInt32()
                Dim unknown As Integer = br.ReadInt32()
                br.ReadBytes(16)
                Dim name As String = New String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetChars(br.ReadBytes(nameSize)))
                br.ReadByte()
                Dim bw As New BinaryWriter(File.Create(path & "//" & name))
                bw.Write(br.ReadBytes(fileSize))
                bw.Close()
            Next
        End Sub
    End Class


