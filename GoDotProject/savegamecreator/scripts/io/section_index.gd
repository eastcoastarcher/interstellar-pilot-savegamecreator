class_name SectionIndex
extends RefCounted

# Finds length-prefixed [Heading] strings in the save file.
# Returns an Array of {name, offset}, where offset is the byte position of the length prefix.
static func find_headings(path: String) -> Array:
	var bytes := FileAccess.get_file_as_bytes(path)
	var results := []
	for i in range(1, bytes.size()):
		var n: int = bytes[i - 1]
		if n < 3 or i + n > bytes.size():
			continue
		if bytes[i] != 0x5B or bytes[i + n - 1] != 0x5D:
			continue
		var ok := true
		for j in range(1, n - 1):
			var c: int = bytes[i + j]
			var allowed := (c >= 48 and c <= 57) or (c >= 65 and c <= 90) or (c >= 97 and c <= 122) or c == 32 or c == 95 or c == 46 or c == 45
			if not allowed:
				ok = false
				break
		if ok:
			var name := bytes.slice(i + 1, i + n - 1).get_string_from_utf8()
			results.append({"name": name, "offset": i - 1})
	return results
