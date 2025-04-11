## Init project

```bash
cd ./scripts
./init_project.sh
```

After running the `init_project.sh` script, you need to configure couple of things:

* **`CLI_APP_DIR`**: Set this variable in the `.env` file to the absolute path where you want the `wtrack` CLI executable to be placed.
    * *Examples:* `CLI_APP_DIR=/usr/local/bin`, `CLI_APP_DIR=$HOME/bin`, `CLI_APP_DIR=C:/Program Files/wtrack`, etc.

* **Add to PATH**: Add the directory specified in `CLI_APP_DIR` to your system's `PATH` environment variable. This makes the command accessible from anywhere.

**How it works:** A pre-push Git hook is configured in this repository. Every time you successfully `git push`, this hook automatically copies the compiled `wtrack` executable to the location defined by `CLI_APP_DIR`. After setting up your PATH, you can run the tool simply by typing `wtrack` in your terminal.


## CLI App Usage

```
Usage: wtrack [OPTIONS] COMMAND [ARGS]...

Options:
  --help  Show this message and exit.

Commands:
  login
  logout
  add
  get
  update
  remove
  forecast
  show
  ping
```
