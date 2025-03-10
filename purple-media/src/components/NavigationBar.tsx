import NavigationButton from "./NavigationButton"

type NavigationButtonProps = {
    label: string;
    url: string;
};

type NavigationBarProps = {
    navigationButtons: NavigationButtonProps[];
};

function NavigationBar({navigationButtons}: NavigationBarProps) {
    return (
        <div className="flex-1/6 flex flex-col items-center pt-10 pr-5 pl-5 bg-purple-700">
            {navigationButtons.map((button, index) => (
                <NavigationButton key={index} label={button.label} url={button.url} />
            ))}
        </div>
    )
}

export default NavigationBar