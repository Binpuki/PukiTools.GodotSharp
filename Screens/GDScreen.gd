class_name GDScreen extends Node

## A main screen class for GDScript classes.

@export var resources_to_load : ResourceLoadList = ResourceLoadList.new() ## Resources that will be loaded upon entering a loading screen.

func ready_preload() -> void: ## Triggers right after the scene is loaded to add resources to load.
	_preloaded = true

func on_resource_loaded(_path : String) -> void: ## Triggers upon loading a resource specified in [member resources_to_load]
	pass

var _preloaded : bool = false

func _ready() -> void:
	if _preloaded:
		return
	
	ScreenManager.SwitchScreen(scene_file_path, "default")

func is_preloaded() -> bool: ## Whether this screen was preloaded first.
	return _preloaded
