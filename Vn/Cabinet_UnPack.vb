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
            Dim signature As String = Encoding.ASCII.GetString(br.ReadBytes(3)) ' file signature
            Dim reserved1 As UInteger = br.ReadUInt32 ' reserved
            Dim cbCabinet As UInteger = br.ReadUInt32' size of this cabinet file in bytes
            Dim reserved2 As UInteger = br.ReasUInt32 ' reserved
            Dim coffFiles As UInteger = br.ReadUInt32 ' offset of the first CFFILE entry
            Dim reserved3 As UInteger = br.ReadUInt32 ' reserved
            Dim versionMinor As Byte = br.ReadByte ' cabinet file format version, minor
            Dim versionMajor As Byte = br.ReadByte ' cabinet file format version, major
            Dim cFolders As UShort = br.ReadUInt16 ' number of CFFOLDER entries in this cabinet
            Dim cFiles As UShort = br.ReadUInt16 ' number of CFFILE entries in this cabinet
            Dim flags As UShort = br.ReadUInt16 ' cabinet file option indicators
            Dim setID As UShort = br.ReadUInt16 ' must be the same for all cabinets in a set
            Dim iCabinet As UShort = br.ReadUInt16 ' number of this cabinet file in a set
            Dim cbCFHeader As UShort = br.ReadUInt16 ' (optional) size of per-cabinet reserved area
            Dim cbCFFolder As Byte = br.ReadByte ' (optional) size of per-folder reserved area
            Dim cbCFData As Byte = br.ReadByte ' (optional) size of per-datablock reserved area
            Dim abReserve As Byte() ' (optional) per-cabinet reserved area
            Dim szCabinetPrev As Byte() ' (optional) name of previous cabinet file
            Dim szDiskPrev As Byte() ' (optional) name of previous disk
            Dim szCabinetNext As Byte() ' (optional) name of next cabinet file
            Dim szDiskNext As Byte() ' (optional) name of next disk

            Dim subtables As New List(Of TableData)()
            For i As UInt16 = 0 To cFolders - 1
                subtables.Add(
                    New TableData
                )
            Next

            Dim subfiles As New List(Of FileData)()        
            For j as UInt16 = 0 To cFiles -1
                subfiles.Add(
                    New FileData
                )
            Next

            Dim subdatas as New List(Of CFDATA)
            For Each q As TableData In subtables
              For k as UInt16 = 0 To q.cCFData - 1
                  subdatas.Add(
                    New CFDATA
                )
              Next
            Next

        End If
        Console.ReadLine()
    End Sub

    Class TableData
      Public coffCabStart As UInt32 ' offset of the first CFDATA block in this folder
      Public cCFData As UInt16 ' number of CFDATA blocks in this folder
      Public typeCompress As UInt16 ' compression type indicator
      Public abReserve As Byte() ' (optional) per-folder reserved area

        Public Sub New()
            coffCabStart = br.ReadUInt32
            cCFData = br.ReadUInt32
            typeCompress = br.ReadUInt16
            'abReserve 
        End Sub
    End Class

    Class FileData
      Public cbFile As UInt32 ' uncompressed size of this file in bytes
      Public uoffFolderStart As UInt32 ' uncompressed offset of this file in the folder
      Public iFolder As UInt16 ' index into the CFFOLDER area
      Public [date] As UInt16 ' date stamp for this file
      Public time As UInt16 ' time stamp for this file
      Public attribs As UInt16 ' attribute flags for this file
      Public szName As String ' name of this file

        Public Sub New()
            cbFile = br.ReadUInt32
            uoffFolderStart = br.ReadUInt32
            iFolder = br.ReadUInt16
            [date] = br.ReadUInt16
            time = br.ReadUInt16
            attribs = br.ReadUInt16
            szName = br.ReadString
        End Sub
    End Class

    Class CFDATA
      Public csum As UInt32 ' checksum of this CFDATA entry
      Public cbData As UInt16 ' number of compressed bytes in this block
      Public cbUncomp As UInt16 ' number of uncompressed bytes in this block
      Public abReserve As Byte() ' (optional) per-datablock reserved area
      Public ab As Byte() ' compressed data bytes

         Public Sub New()
             csum = br.ReadUInt32
             cbData = br.ReadUInt16
             cbUncomp br.ReadUInt16
             'abReserve = 
             'ab = 
         End Sub
    End Class

End Module
