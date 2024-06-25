Dim sign as String = New String(br.ReadChars(4))
Dim vers1 as Int16 = br.ReadaInt16 ' Version MadeBy
Dim vers2 as Int16 = br.ReadInt16 ' Version to Decompress
Dim flags as Int16 = br.ReadInt16
Dim types as Int16 = br.ReadInt16 ' Compression Type
Dim modtime as Int16 = br.ReadInt16
Dim modDate as Int16 = br.ReadInt16
Dim crc32 as Int32 = br.ReadInt32
Dim sizeCompressed as Int32 = br.ReadInt32
Dim sizeUncompressed as Int32 = br.ReadInt32
Dim filenamelength as Int16 = br.ReadInt16
Dim extrafieldlength as Int16 = br.ReadInt16
Dim filecommentlength as Int16 = br.ReadInt16
Dim diskNumberStart as Int16 = br.ReadInt16
Dim internalFileAttributes as Int16 = br.ReadInt16
Dim externalFileAttributes as Int32 = br.ReadInt32
Dim relativeOffsetOfLocalHeader as Int32 = br.ReadInt32


