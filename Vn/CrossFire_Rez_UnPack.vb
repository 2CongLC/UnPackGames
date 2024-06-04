' https://gist.github.com/kungfulon/dfa49323eb7a55db964f10174e57c19f
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
            br.Read(buffer, 0, RootDirSize)
            Decrypt(buffer, RootDirSize, RootDirPos)
            Dim ms as MemoryStream = New MemoryStream(buffer)
            ms.Position = 0
            br = New BinaryReader(ms)
            Dim types as Int32 = br.ReadInt32
            br.BaseStream.Position += 4
            Console.WriteLine("Data Type : {0}",types)
            Dim subtables as New List(Of TableData)()
            Dim subfiles as New List(Of FileData)()
            If Types = 1 Then
                subtables.Add(New TableData)
                br.BaseStream.Position += 16
            Else
                subfilea.Add(New FileData)
                br.BaseStream.Position += 28
            End If 

            p = Path.GetDirectoryName(input) & "\" & Path.GetFileNameWithoutExtension(input)
            
            Dim n as String = Nothing
            Dim extension as String = Nothing

            For Each td as TableData in subtables
                n = td.name
            Next
            Directory.CreateDirectory(p)
            For Each fd as FileData in subfiles
                Dim ext As String = New String(BitConverter.GetBytes(fd.ext).Reverse().Select(Function(c) CChar(c)).ToArray())
                Dim buffer as Byte() = br.ReadBytes(fd.size)
                br.BaseStream.Position = fd.pos
                Using bw As New BinaryWriter(File.Create(p & "//" & fd.id))
                  bw.Write(br.ReadBytes(fd.size))
                End Using                   
            Next
            Console.WriteLine("unpack done ")    
        End If
    End Sub

    Class TableData
        Dim pos as UInt32 ' Length = 4
        Dim size as UInt32 ' Length = 4
        Dim time as UInt32 ' Length = 4
        Dim namelen as String ' Length = 4
        Public Sub New()
            pos = br.ReadUInt32
            size = br.ReadUInt32
            time = br.ReadUInt32
            namelen = New String(BitConverter.GetBytes(br.ReadUInt32))
        End Sub
    End Class
    
    Class FileData
        Dim pos as UInt32 ' Length = 4
        Dim size as UInt32 ' Length = 4
        Dim time as UInt32 ' Length = 4
        Dim id as UInt32 ' Length = 4
        Dim ext as UInt32 ' Length = 4
        Dim numkeys as UInt32 ' Length = 4
        Dim namelen as String ' Length = 4
        Public Sub New()
            pos = br.ReadUInt32
            size = br.ReadUInt32
            time = br.ReadUInt32
            id = br.ReadUInt32
            ex = br.ReadUInt32
            numkeys = br.ReadUInt32
            namelen = New String(BitConverter.GetBytes(br.ReadUInt32))
        End Sub    
    End Class

    Sub Decrypt(ByRef buffer() As Byte, ByVal size As Integer, ByVal keyPos As Integer)
        For i As Integer = 0 To size - 1
            buffer(i) = &H49 + (KEYS(keyPos Mod KEYS.Length) Xor Not buffer(i))
            keyPos += 1
        Next
    End Sub

    Dim KEYS() As Byte = {
    &HF0, &HF0, &H9D, &H09, &H0A, &H66, &HAD, &H6A, &H85, &H1D, 
    &HFD, &H3F, &H51, &H23, &HE7, &HF3, &HB1, &H0E, &H78, &HEC, 
    &HD1, &H50, &H7B, &H6B, &H17, &H3F, &H61, &HC5, &H79, &H0C, 
    &H57, &H32, &H1A, &HF3, &HB8, &H6B, &H68, &HDE, &H2A, &H5F, 
    &H01, &HBA, &H98, &H3A, &H99, &HC0, &H54, &H02, &H24, &HF7, 
    &H9B, &H09, &H87, &H23, &HC4, &H6F, &H0E, &H6C, &H44, &HFA, 
    &HDB, &HFB, &HE8, &H85, &HAB, &HC2, &H65, &H3C, &H0E, &HC4, 
    &H93, &HF6, &H6D, &H0B, &H8A, &HD6, &H11, &H8D, &HE3, &H8F, 
    &H71, &H52, &H5D, &H6E, &HFC, &HFD, &H29, &H82, &HB0, &H1D, 
    &H13, &H11, &HAE, &H5C, &HD5, &HA9, &H1B, &HF8, &HCE, &HFC, 
    &H79, &H9C, &H5A, &HD6, &HCE, &HFD, &H0C, &H64, &HCA, &H60, 
    &H16, &H12, &H31, &H5B, &H08, &H3A, &HCF, &H04, &H3E, &HEA, 
    &H23, &HDC, &H28, &HFA, &H20, &HA5, &HC0, &HB8, &H21, &H73, 
    &H5E, &H6C, &H6A, &H2B, &H31, &HE9, &H6D, &HBD, &H9A, &H73, 
    &H11, &H4C, &HB1, &H43, &H3A, &H8E, &H28, &HCE, &HDC, &H9B, 
    &HD4, &H31, &HCF, &H77, &H1D, &HE4, &H9F, &H8A, &H8B, &H0A, 
    &HB2, &H4E, &HC0, &H8D, &HDD, &H74, &H0B, &H56, &HCF, &HB7, 
    &HEE, &HD5, &H74, &HA7, &HB5, &H1B, &HA1, &HA9, &H85, &HCB, 
    &H45, &H68, &HFF, &H1F, &H59, &HFB, &HCD, &H42, &HDA, &HFF, 
    &H59, &H37, &H05, &HE7, &HDC, &H9E, &H12, &HBD, &H1B, &H87, 
    &HBB, &H97, &H02, &H9A, &HC2, &H04, &H66, &HD3, &HBE, &HA7, 
    &H2C, &H11, &H66, &H4E, &H10, &HBD, &HA8, &HB3, &H54, &HC2, 
    &HC0, &H39, &H8D, &H17, &H91, &HDA, &HE0, &H21, &H86, &H8A, 
    &HD3, &H24, &H37, &H4A, &H10, &H13, &H0A, &H38, &H45, &HE2, 
    &H26, &HC6, &H66, &HC0, &HDE, &H73, &H9B, &H53, &HE2, &H2D, 
    &H0A, &H57, &H7E, &HAC, &HC9, &HC4, &H0C, &H04, &H33, &HD5, 
    &HFA, &H9F, &HE5, &H15, &H8A, &HFD, &H95, &HCF, &H9A, &H57, 
    &H16, &H02, &HB2, &H81, &HBE, &H39, &H8C, &H3A, &H72, &H6A, 
    &H6F, &H34, &H8A, &H2F, &H84, &H0E, &HEE, &H96, &H6D, &H80, 
    &H83, &HBC, &H6A, &H02, &H45, &H84, &H3A, &H1C, &H49, &HA0, 
    &H01, &HB7, &HDA, &H2C, &H76, &H96, &HFF, &H1D, &H8E, &H49, 
    &HA7, &HCA, &HF5, &HD6, &HB0, &HBD, &H7F, &H51, &H21, &H25, 
    &HEA, &HAC, &HB7, &H15, &H16, &HF6, &H24, &HD7, &H0E, &H54, 
    &H27, &H96, &H0D, &HEC, &HD4, &H96, &HC9, &H00, &H33, &H4D, 
    &H43, &H83, &H8C, &H7B, &H59, &H5E, &H96, &HAF, &H5F, &HAC, 
    &HC3, &H4A, &HF9, &H23, &HFC, &H62, &H7B, &HFF, &HF5, &HB9, 
    &H0C, &H91, &H6A, &H01, &HCD, &HC9, &H87, &HBB, &H43, &HFC, 
    &HA4, &HE7, &H49, &H0D, &HB5, &HC7, &HC3, &H5A, &H95, &HF7, 
    &H52, &H91, &H78, &H1D, &H52, &HC4, &HBC, &H63, &H5A, &HE4, 
    &H6A, &H11, &H7B, &HFF, &H8D, &H72, &H8E, &H64, &HB5, &H53, 
    &HB8, &H07, &HDD, &H4E, &H7F, &H4D, &HF4, &H35, &H99, &H96, 
    &H4A, &HC6, &HC6, &HB7, &H20, &HF6, &HEB, &HA9, &HA1, &H18, 
    &HAF, &HA7, &H77, &H07, &HE2, &H0B, &H49, &HBA, &HE1, &H12, 
    &H60, &H55, &H41, &HDD, &HA8, &H21, &H03, &HE5, &H5B, &H8F, 
    &H81, &H1E, &H8D, &H8B, &H6A, &H11, &HE0, &H6F, &HF9, &H2F, 
    &H96, &HC1, &HBA, &H8E, &H4D, &H06, &H06, &H62, &H9A, &HE8, 
    &H92, &H66, &HCC, &HFB, &H34, &H7B, &H11, &H42, &H34, &HBC, 
    &H3D, &HDC, &H63, &H3E, &H7A, &HF7, &H2C, &HD4, &H19, &H60, 
    &HF5, &HF3, &HC5, &HE1, &HF9, &H1D, &H5F, &HB4, &HEF, &HEF, 
    &HBA, &H4E, &HB1, &H35, &H7B, &HBD, &H26, &H1D, &H61, &HD0, 
    &HB0, &HF4, &H2C, &H65, &H64, &H84, &H6B, &HFB, &H3C, &H74, 
    &H6D, &HE1, &H93, &HD2, &H98, &H36, &H2A, &H18, &H5F, &HFA, 
    &HE2, &HE1, &H23, &H7C, &H8C, &H93, &H2E, &H53, &HEE, &H40, 
    &H23, &H2C, &H56, &HF3, &HFB, &HB3, &HEC, &HBC, &HFA, &HC7, 
    &H06, &HA6, &HC0, &H4B, &HCC, &HE8, &HBB, &HC1, &H4C, &H84, 
    &H41, &H01, &H67, &HA2, &H8F, &H43, &HB2, &HD6, &HEA, &HB6, 
    &HA4, &HA0, &H21, &HF7, &H45, &H5E, &HBC, &H8E, &H9F, &HF2, 
    &H03, &HCC, &H3B, &H5F, &H35, &H36, &HD4, &H91, &H18, &HC3, 
    &H9E, &HA6, &H36, &H32, &H44, &HE0, &HFA, &HB2, &HF1, &H91, 
    &HEF, &H1F, &H9D, &H39, &H66, &H10, &HDA, &H18, &HC2, &HFE, 
    &H66, &H73, &H9F, &HBA, &HC8, &HD2, &H2C, &H7B, &H23, &H6A, 
    &HD9, &HBD, &H9E, &H02, &HB2, &H35, &H7E, &H87, &H9E, &H1B, 
    &H58, &H9A, &HC1, &H06, &H70, &H49, &H3D, &H9A, &HB4, &H46, 
    &H9F, &H4D, &H67, &HCB, &H2A, &H82, &HDC, &H75, &H4A, &H32, 
    &H70, &H50, &H68, &H6E, &H0A, &H5C, &H65, &HF2, &H5E, &HC4, 
    &HF6, &H0E, &H34, &H04, &H23, &H24, &HF3, &H4B, &H30, &HF3, 
    &HB2, &H4E, &H26, &H02, &H07, &HC8, &H3D, &H54, &HE5, &HFB, 
    &H6F, &HB4, &HB0, &H5E, &H71, &HD8, &HE1, &HB9, &H44, &H92, 
    &H69, &H02, &HBB, &H5C, &H16, &H24, &H16, &H70, &H3E, &HFD, 
    &H09, &HBD, &HF2, &HD2, &H69, &HE7, &HEE, &H74, &HB3, &HA1, 
    &H92, &H5A, &HC0, &H99, &H1A, &HF2, &HDD, &H3A, &H62, &H5E, 
    &H81, &H7D, &H66, &HF0, &HE9, &H14, &HCA, &H8F, &HDD, &H24, 
    &HA6, &H5A, &HD4, &HD8, &HD3, &HB8, &HBB, &H03, &H03, &H1D, 
    &HA6, &H19, &HD1, &HC6, &H9E, &HBA, &H25, &HA8, &HD8, &H16, 
    &H0B, &HCF, &H8D, &H5C, &H5B, &H78, &HB9, &H88, &H60, &H19, 
    &HFB, &HB8, &HC1, &HA0, &HD9, &H65, &HF3, &H24, &HAF, &H9F, 
    &H6A, &H4F, &H72, &HAC, &HD2, &HB3, &HAC, &H2F, &H87, &H5C, 
    &HCB, &H2B, &H9A, &HD0, &H1C, &H18, &H8F, &HC7, &HA7, &H47, 
    &H26, &HD6, &H32, &HE5, &H68, &H4A, &HA5, &HC4, &H31, &H7C, 
    &H16, &H44, &H8C, &HD8, &HB0, &H8C, &H01, &HD6, &HCD, &H51, 
    &H37, &H2B, &H62, &H7B, &H0F, &H66, &H20, &HD8, &H88, &H4B, 
    &H6C, &H23, &HAB, &H1C, &H84, &HA2, &HAF, &H15, &H01, &H95, 
    &HAC, &H62, &H03, &HBB, &H0F, &HC2, &H3C, &H29, &H0F, &H24, 
    &H22, &HB9, &H6B, &H72, &H86, &H46, &HA6, &HD6, &HCB, &H06, 
    &H0E, &HB0, &H04, &H2C, &HBD, &H7E, &H35, &H29, &HED, &HFE, 
    &HF9, &HB9, &HC1, &HBC, &HC9, &H0A, &HD8, &H5B, &H2F, &H33, 
    &HE9, &HD0, &H0F, &H3E, &H9A, &HCC, &H63, &H0C, &HE0, &HA3, 
    &H91, &H4A, &H25, &HE1, &HA9, &HB3, &H6B, &HD2, &HC6, &HF2, 
    &HBA, &H41, &HD5, &H51, &H0F, &HAE, &HFB, &H7C, &H0F, &H30, 
    &HE4, &H9A, &HBE, &H50, &H36, &HF9, &H7A, &H17, &H62, &H8E, 
    &H7B, &H94, &H23, &H8C, &H15, &H0C, &HD5, &H48, &H02, &H2B, 
    &HFB, &HB6, &HEB, &H5B, &H22, &HBE, &H75, &H9E, &H6A, &H99, 
    &H1A, &H0D, &HF6, &H90, &HFC, &H57, &H79, &H43, &H01, &H6F, 
    &H2F, &HCD, &H74, &HAB, &H74, &HF5, &H65, &H9D, &H43, &HBB, 
    &H13, &HDE, &HD5, &H6D, &H97, &H08, &HA9, &H9E, &H11, &H2E, 
    &H2A, &H29, &HA0, &HFD, &H3F, &H84, &H52, &HDB, &HFB, &HB4, 
    &H67, &H30, &HB3, &H08, &H0B, &H2D, &HB7, &HEE, &HDA, &H41, 
    &HED, &H1C, &H6A, &H7F, &H98, &H4F, &H14, &H45, &H75, &HD4, 
    &H42, &H44, &H8C, &H34, &H86, &H4F, &HD9, &H28, &HAF, &H10, 
    &H1E, &H25, &H22, &HF7, &H1A, &HC0, &HBE, &HA0, &H5D, &H1E, 
    &H7C, &HE3, &H0F, &HBE, &H17, &HE4, &HC5, &HD5, &HF9, &H4D, 
    &HD0, &H7F, &HA7
}




    

End Module      
