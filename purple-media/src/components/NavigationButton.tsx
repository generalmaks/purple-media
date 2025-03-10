interface NavigationButtonProps {
    label: string;
    url: string;
}

const NavigationButton: React.FC<NavigationButtonProps> = ({ label, url }) => {
    return (
        <a href={url} className="btn text-4xl p-5 border-4 rounded-2xl mb-3 w-full text-center">
            {label}
        </a>
    )
}

export default NavigationButton