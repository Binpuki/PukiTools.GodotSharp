class_name GDScreen extends Node

## A main screen class for GDScript classes.

@export var resources_to_load : Array[String] = [] ## Resources that will be loaded upon entering a loading screen.
@export var needs_preloading : bool = false ## Check this to make SURE things preload. Useful in the case of hitting "Play Current Scene" on a screen.  

func ready_preload() -> void: ## Triggers right after the scene is loaded to add resources to load.
	_preloaded = true

func on_resource_loaded(_path : String) -> void: ## Triggers upon loading a resource specified in [member resources_to_load]
	pass

var _preloaded : bool = false

func _ready() -> void:
	if is_loaded():
		return
	
	ScreenManager.SwitchScreen(scene_file_path, "default")

func get_debug_info() -> String: ## Can be used by debug displays to get needed info for testing purposes.
	return ""

func is_loaded() -> bool: ## Whether this screen was loaded properly.
	return not needs_preloading or (needs_preloading and _preloaded)
