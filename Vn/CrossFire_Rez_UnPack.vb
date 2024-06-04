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
            Dim RootDirSize As UInteger
            Dim RootDirTime As UInteger
            Dim NextWritePos As UInteger
            Dim Time As UInteger
            Dim LargestKeyAry As UInteger
            Dim LargestDirNameSize As UInteger
            Dim LargestRezNameSize As UInteger
            Dim LargestCommentSize As UInteger
            Dim IsSorted As Byte





        End If
    End Sub
    Class FileData

    End Class

    

End Module      
