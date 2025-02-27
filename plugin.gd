@tool

extends EditorPlugin

var item_group : PackedStringArray = [
	"\t<ItemGroup Label=\"PukiTools.GodotSharp\">",
	"\t\t<ProjectReference Include=\"addons\\PukiTools.GodotSharp\\PukiTools.GodotSharp.SourceGenerators\\PukiTools.GodotSharp.SourceGenerators.csproj\" OutputItemType=\"Analyzer\" ReferenceOutputAssembly=\"false\" />",
	"\t\t<EmbeddedResource Remove=\"addons\\PukiTools.GodotSharp\\PukiTools.GodotSharp.SourceGenerators\\**\" />",
	"\t\t<Compile Remove=\"addons\\PukiTools.GodotSharp\\PukiTools.GodotSharp.SourceGenerators\\**\" />",
	"\t\t<Compile Remove=\"addons\\PukiTools.GodotSharp\\UserSettingsData.cs\" />",
	"\t</ItemGroup>"
]

func _enable_plugin() -> void:
	if not Engine.is_editor_hint():
		return
	
	var csproj_path : String = get_csproj_path()
	var text_lines : Array = Array(FileAccess.get_file_as_string(csproj_path).split("\n"))
	var has_item_group : bool = check_item_group_exists(text_lines)
	if has_item_group:
		return
		
	write_item_group(text_lines)
	var writer : FileAccess = FileAccess.open(csproj_path, FileAccess.WRITE)
	for i in text_lines.size():
		var text : String = text_lines[i] + ("\n" if i != text_lines.size() - 1 else "")
		writer.store_string(text)
	
	writer.close()

func _disable_plugin() -> void:
	if not Engine.is_editor_hint():
		return
	
	var csproj_path : String = get_csproj_path()
	var text_lines : Array = Array(FileAccess.get_file_as_string(csproj_path).split("\n"))
	remove_item_group(text_lines)
	var writer : FileAccess = FileAccess.open(csproj_path, FileAccess.WRITE)
	for i in text_lines.size():
		var text : String = text_lines[i] + ("\n" if i != text_lines.size() - 1 else "")
		writer.store_string(text)
	
	writer.close()

func get_project_settings_path() -> String:
	return "res://project.godot"

func get_csproj_path() -> String:
	var solution_directory : String = ProjectSettings.get_setting("dotnet/project/solution_directory")
	var assembly_name : String = ProjectSettings.get_setting("dotnet/project/assembly_name")
	return solution_directory + assembly_name + ".csproj"

func check_item_group_exists(text_lines : Array) -> bool:
	return text_lines.has(item_group[0])

func write_item_group(text_lines : Array) -> void:
	var last_line : String = text_lines.pop_back()
	for text in item_group:
		text_lines.push_back(text)
	text_lines.push_back(last_line)

func remove_item_group(text_lines : Array) -> void:
	var index_of_group : int = text_lines.find(item_group[0])
	var line_count : int = item_group.size()
	while line_count > 0:
		text_lines.remove_at(index_of_group)
		line_count -= 1
