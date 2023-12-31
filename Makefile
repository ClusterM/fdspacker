APP_NAME=fdspacker
PROJECT_PATH=FDSPacker
SUPPORTED_ARCHS=win-x86 win-x64 win-arm linux-x64 linux-arm osx-x64 osx-arm64

OUTPUT_DIR?=release
CONFIGURATION?=Release
SELF_CONTAINED?=0
ARCH?=win-x64

COMMIT=$(shell git rev-parse --short HEAD)
COMMIT_RESOURCE=$(PROJECT_PATH)/commit.txt
BUILDTIME_RESOURCE=$(PROJECT_PATH)/buildtime.txt

ifeq ($(SELF_CONTAINED),0)
	SC_OPS=--no-self-contained
	SC_NAME=
else
	SC_OPS=--self-contained true -p:PublishTrimmed=True
	SC_NAME=-self-contained
endif

build-all:
	for arch in $(SUPPORTED_ARCHS); do make build ARCH=$$arch ; make build ARCH=$$arch SELF_CONTAINED=1 ; done

release: clean build-all
	for arch in $(SUPPORTED_ARCHS); do make archive ARCH=$$arch && make archive ARCH=$$arch SELF_CONTAINED=1; done

commit:
	echo -n $(COMMIT) > $(COMMIT_RESOURCE)
	git diff-index --quiet HEAD -- || echo -n " (dirty)" >> $(COMMIT_RESOURCE)

buildtime:
	date -u +"%s" > $(BUILDTIME_RESOURCE)

clean:
	rm -rf $(OUTPUT_DIR) $(COMMIT_RESOURCE) $(BUILDTIME_RESOURCE)
	dotnet clean

build:
	dotnet publish $(PROJECT_PATH) -c $(CONFIGURATION) -r $(ARCH) -p:PublishSingleFile=true $(SC_OPS) -o $(OUTPUT_DIR)/$(ARCH)$(SC_NAME)/$(APP_NAME)

archive:
	cd $(OUTPUT_DIR)/$(ARCH)$(SC_NAME) && tar -czvf ../$(APP_NAME)-$(ARCH)$(SC_NAME).tar.gz $(APP_NAME)
	cd $(OUTPUT_DIR)/$(ARCH)$(SC_NAME) && zip -r9 ../$(APP_NAME)-$(ARCH)$(SC_NAME).zip $(APP_NAME)
