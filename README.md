# FDSPacker
It's a simple CLI tool to pack and unpack .fds (Famicom Disk System disk images) files: extract/combine metadata and file. Useful for FDS developers.

## Usage
There are two commands: `pack` and `unpack`:
```
 fdspacker.exe pack [options] <diskinfo.json> <output.fds>
  Options:
   -d, --header       - add header to the output .fds file
   -q, --quiet        - do not print anything to the console
 fdspacker.exe unpack [options] <input.fds> <output directory>
  Options:
   -u, --no-unknown   - do not extract unknown fields
   -q, --quiet        - do not print anything to the console
```

FDSPacker stores all metadata to `diskinfo.json` file. Example:
```
{
  "sides": [        // Array of disk sides. Can be >2 in case of multiple disks.
    {
      "licensee_code": "Konami", // Licensee (manufacturer) code. Can be string or integer value. 
                                 // See https://github.com/ClusterM/nes-containers/blob/7bd25fb863591bd91de0edf4d5d97c3401b9edaf/FdsBlockDiskInfo.cs#L58
                                 // for known values.
      "game_name": "AIN",        // 3-letter ASCII code per game (e.g. ZEL for The Legend of Zelda).
      "game_type": " ",          // Game type: " " — Normal disk, "E" — Event, "R" — Reduction in price.
                                 // Can be character, integer (0-255) value.
      "game_version": "0",       // Game version. Starts at $00, increments per revision. Integer.
      "disk_side": "A",          // Disk side: "A" or "B".
      "disk_number": "0",        // Disk number. First disk is $00, second is $01, etc. Can be integer.
      "disk_type": "FMS",        // Disk type: "FMS" or (normal/other) or "FSC". You can use integer/HEX values too.
      "unknown01": "$00",        // Optional unknown value at offset $16. Usually/default is $00. Integer.
      "boot_file": "15",         // Boot read file code, Refers to the file code/file number to load upon boot/start-up. Integer.
      "unknown02": "$FF",        // Optional unknown value at offset $1A. Usually/default is $FF. Integer.
      "unknown03": "$FF",        // Optional unknown value at offset $1B. Usually/default is $FF. Integer.
      "unknown04": "$FF",        // Optional unknown value at offset $1C. Usually/default is $FF. Integer.
      "unknown05": "$FF",        // Optional unknown value at offset $1D. Usually/default is $FF. Integer.
      "unknown06": "$FF",        // Optional unknown value at offset $1E. Usually/default is $FF. Integer.
      "manufacturing_date": "1987-04-17T00:00:00",  // Manufacturing date.
      "country_code": "Japan",   // Country code. Can be "Japan" or integer.
      "unknown07": "$FF",        // Optional unknown value at offset $23. Usually/default is $61. Speculative: Region code? Integer.
      "unknown08": "$FF",        // Optional unknown value at offset $24. Usually/default is $00. Speculative: Location/site? Integer.
      "unknown09": "$FF",        // Optional unknown value at offset $25. Usually/default is $00. Integer.
      "unknown10": "$FF",        // Optional unknown value at offset $26. Usually/default is $02. Integer.
      "unknown11": "$00",        // Optional unknown value at offset $27. Default is $00. Speculative: some kind of game information? Integer.
      "unknown12": "$00",        // Optional unknown value at offset $28. Default is $00. Speculative: some kind of game information? Integer.
      "unknown13": "$00",        // Optional unknown value at offset $29. Default is $00. Speculative: some kind of game information? Integer.
      "unknown14": "$00",        // Optional unknown value at offset $2A. Default is $00. Speculative: some kind of game information? Integer.
      "unknown15": "$00",        // Optional unknown value at offset $2B. Default is $00. Speculative: some kind of game information? Integer.
      "rewritten_date": null,    // "Rewritten disk" date. Can be null or same as "manufacturing_date" in the case of an original (non-copied) disk.
      "unknown16": "$00",        // Optional unknown value at offset $2F. Default is $00. Integer.
      "unknown17": "$80",        // Optional unknown value at offset $30. Usually/default is $80.
      "disk_writer_serial_number": "$FFFF",  // Disk Writer serial number. Integer (0-65535 or $0000-$FFFF).
      "unknown18": "$80",        // Optional unknown value at offset $33. Usually/default $07. Integer.
      "disk_rewrite_count": "0", // Disk rewrite count
      "actual_disk_side": "A",   // Actual disk side (???): "A" or "B".
      "disk_type_other": "YellowDisk",  // Disk type (other). Can be string or integer. $00 - yellow disk, $FF - blue disk, $FE - prototype.
      "disk_version": "$03",     // Unknown how this differs from game version. Disk version numbers indicate different software revisions.
                                 // Speculation is that disk version incremented with each disk received from a licensee. Integer.
      "file_amount": 10,         // Visible file amount. Usually it equals to actual file amount but it may vary (for copy protection, etc.)
      "files": [                 // Array of files on the disk.
        {
          "file_number": "0",           // File number. Must go in increasing order, first file is 0. Integer.
          "file_indicate_code": "0",    // The ID specified at disk-read function call. Integer.
          "file_name": "KYODAKU-",      // File name. Maximum is 8 letters.
          "file_address": "$2800",      // The destination address when loading. Integer (0-65535 or $0000-$FFFF).
          "file_kind": "NameTable",     // File type: "Program", "Character" or "NameTable".
          "data": "side_1/kyodaku-.nt"  // Path to the file with data. It will be loaded with this .json file and stored in .fds as file contents.
        },
       // ...
       {
          "file_number": "12",
          "file_indicate_code": "19",
          "file_name": "ZX606P_3",
          "file_address": "$6000",
          "file_kind": "Program",
          "data": "side_2/zx606p_3.prg"
        }
      ]
    }
  ]
}
```
All integer values can be stored as hexadecimal value: `$FF` or `0xFF`. You can use `--no-unknown` option when unpacking to ignore all unknown fields.

## Donate
* [Buy Me A Coffee](https://www.buymeacoffee.com/cluster)
* [Donation Alerts](https://www.donationalerts.com/r/clustermeerkat)
* [Boosty](https://boosty.to/cluster)
* BTC: 1MBYsGczwCypXhMBocoDQWxx7KZT2iiwzJ
* PayPal is not available in Armenia :(
