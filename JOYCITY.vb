'Written for games by JOYCITY
'FreeStyleFootball https://steamcommunity.com/app/568810/
'FreestyleFootball R https://store.steampowered.com/app/1826980
'Freestyle 2 Street Basketball https://store.steampowered.com/app/339610/
    
Friend Class Program
        Public Shared br As BinaryReader

        Shared Sub Main(ByVal args() As String)
            br = New BinaryReader(File.OpenRead(args(0)))

            If New String(br.ReadChars(4)) <> "PACK" Then
                Throw New Exception("This is not a JOYCITY pak file.")
            End If

            Dim tableStart As Integer = br.ReadInt32()
            Dim tableSize As Integer = br.ReadInt32()

            br.BaseStream.Position = tableStart 'We're already there, but just in case.
            Dim subfiles As New List(Of Subfile)()
            While br.BaseStream.Position < tableStart + tableSize
                subfiles.Add(New Subfile())
            End While

            Dim path As String = Path.GetDirectoryName(args(0)) & "//" & Path.GetFileNameWithoutExtension(args(0)) & "//"
            For Each file As Subfile In subfiles
                Directory.CreateDirectory(path & Path.GetDirectoryName(file.name))
                br.BaseStream.Position = file.start
                Dim bw As New BinaryWriter(File.Create(path & file.name))
                bw.Write(br.ReadBytes(file.size))
                bw.Close()
            Next
        End Sub

        Private Class Subfile
            Public name As String = New String(br.ReadChars(256)).TrimEnd(ControlChars.NullChar)
            Public start As Integer = br.ReadInt32()
            Public size As Integer = br.ReadInt32()
        End Class
    End Class


