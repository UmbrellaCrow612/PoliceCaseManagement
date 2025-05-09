import json
import os
from jinja2 import Environment, FileSystemLoader

def main():
    # Get the current directory where the script is located
    current_dir = os.path.dirname(os.path.realpath(__file__))

    # Define the paths relative to the current directory
    config_file_path = os.path.join(current_dir, 'config.json')
    template_file_path = os.path.join(current_dir, 'nginx.conf.j2')
    output_nginx_conf_path = os.path.join(current_dir, 'nginx.conf')

    print(f"Loading configuration from: {config_file_path}")
    print(f"Loading template from: {template_file_path}")
    print(f"Outputting Nginx config to: {output_nginx_conf_path}")

    # Load the configuration file
    try:
        with open(config_file_path, 'r') as f:
            config_data = json.load(f)
    except FileNotFoundError:
        print(f"Error: Config file not found at {config_file_path}")
        exit(1)
    except json.JSONDecodeError:
        print(f"Error: Could not decode JSON from {config_file_path}")
        exit(1)

    # Set up Jinja2 environment to load templates from the current directory
    env = Environment(loader=FileSystemLoader(current_dir), trim_blocks=True, lstrip_blocks=True)

    # Load the template
    try:
        template = env.get_template('nginx.conf.j2')
    except Exception as e:
        print(f"Error loading template nginx.conf.j2: {e}")
        exit(1)

    # Render the template with the configuration data
    rendered_config = template.render(config_data)

    # Write the rendered Nginx configuration to the output file
    try:
        with open(output_nginx_conf_path, 'w') as f:
            f.write(rendered_config)
        print(f"Nginx configuration successfully generated at {output_nginx_conf_path}")
    except IOError:
        print(f"Error: Could not write Nginx configuration to {output_nginx_conf_path}")
        exit(1)

if __name__ == '__main__':
    main()
