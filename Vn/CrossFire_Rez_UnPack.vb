Imports System
Imports System.Collections
Imports System.IO
Imports System.IO.Compression
Imports System.Runtime
Imports System.Text
Imports System.Text.RegularExpressions


Module Program

    Public br As BinaryReader
    Public input As String
    Sub Main(args As String())
        If args.Count = 0 Then
            Console.WriteLine("Tool UnPack - 2CongLC.vn :: 2024")
        Else
            input = args(0)
        End If
        Dim p As String = Nothing
        If IO.File.Exists(input) Then

            br = New BinaryReader(File.OpenRead(input))
            Dim CR1 As Char = br.ReadChar ' Offset = 0, Length = 2
            Dim LF1 As Char = br.ReadChar ' Offset = 2, Length = 2
            Dim FileType As String = New String(br.ReadChars(60)) ' Offset = 4, Length = 60
            Dim CR2 As Char = br.ReadChar ' Offset = 64, Length = 2
            Dim LF2 As Char = br.ReadChar ' Offset = 66, Length = 2
            Dim UserTitle As String = New String(br.ReadChars(60)) ' Offset = 68, Length = 60
            Dim CR3 As Char = br.ReadChar ' Offset = 128, Length = 2
            Dim LF3 As Char = br.ReadChar ' Offset = 130, Length = 2
            Dim EOF1 As Char = br.ReadChar ' Offset = 132, Length = 2
            Dim FileFormatVersion As UInt32 = br.ReadUInt32 ' Offet = 134, Length = 4
            Dim RootDirPos As UInt32 = br.ReadUInt32 ' Offset = 138, Length = 4
            Dim RootDirSize As UInt32 = br.ReadUInt32 ' Offset = 142, Length = 4
            Dim RootDirTime As UInt32 = br.ReadUInt32 ' Offset = 146, Length = 4
            Dim NextWritePos As UInt32 = br.ReadUInt32 ' Offset = 150, Length = 4
            Dim Time As UInt32 = br.ReadUInt32 ' Offset = 154, Length = 4
            Dim LargestKeyAry As UInt32 = br.ReadUInt32 ' Offset = 158, Length = 4
            Dim LargestDirNameSize As UInt32 = br.ReadUInt32 ' Offset = 162, Length = 4
            Dim LargestRezNameSize As UInt32 = br.ReadUInt32 ' Offset = 166, Length = 4
            Dim LargestCommentSize As UInt32 = br.ReadUInt32 ' Offset = 170, Length = 4
            Dim IsSorted As Byte = br.ReadByte ' Offset = 174, Length = 1

            br.BaseStream.Seek(RootDirPos, SeekOrigin.Begin)
            Dim buffer as Byte() = New Byte(RootDirSize){}
            





        End If
    End Sub

    Class TableData
        Dim pos as UInt32
        Dim size as UInt32
        Dim time as UInt32
        Dim namelen as UInt32
        Public Sub New()
            pos = br.ReadUInt32
            size = br.ReadUInt32
            time = br.ReasUInt32
            namelen = br.ReadUInt32
        End Sub
    End Class
    
    Class FileData
        Dim pos as UInt32
        Dim size as UInt32
        Dim time as UInt32
        Dim id as UInt32
        Dim ex as UInt32
        Dim numkeys as UInt32
        Dim namelen as UInt32
        Public Sub New()
            pos = br.ReadUInt32
            size = br.ReadUInt32
            time = br.ReadUInt32
            id = br.ReadUInt32
            ex = br.ReadUInt32
            numkeys = br.ReadUInt32
            namelen = br.ReadUInt32
        End Sub    
    End Class

    

End Module      
