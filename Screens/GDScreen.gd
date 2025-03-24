class_name GDScreen extends Node

## A main screen class for GDScript classes.

@export var resources_to_load : ResourceLoadList = ResourceLoadList.new() ## Resources that will be loaded upon entering a loading screen.
@export var needs_preloading : bool = false ## Check this to make SURE things preload. Useful in the case of hitting "Play Current Scene" on a screen.  

func ready_preload() -> void: ## Triggers right after the scene is loaded to add resources to load.
	_preloaded = true

func on_resource_loaded(_path : String) -> void: ## Triggers upon loading a resource specified in [member resources_to_load]
	pass

var _preloaded : bool = false

func _ready() -> void:
	if not needs_preloading or (needs_preloading and _preloaded):
		return
	
	ScreenManager.SwitchScreen(scene_file_path, "default")

func is_preloaded() -> bool: ## Whether this screen was preloaded first.
	return _preloaded
